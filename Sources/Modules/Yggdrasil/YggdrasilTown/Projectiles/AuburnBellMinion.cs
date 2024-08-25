using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using rail;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class AuburnBellMinion : ModProjectile
{
	private const float NotMovingVelocity = 1E-05f;
	private const int NoTarget = -1;

	private const float MAXDistanceToPlayer = 1000f;

	private const int TeleportCooldownValue = 60;

	/// <summary>
	/// Summon Number
	/// </summary>
	private int MinionNumber
	{
		get => (int)Projectile.ai[0];
	}

	/// <summary>
	/// Target
	/// </summary>
	private int TargetWhoAmI { get; set; } = NoTarget;

	/// <summary>
	/// Teleport Cooldown
	/// </summary>
	private int TeleportCooldown { get; set; } = 0;

	public override void SetStaticDefaults()
	{
		Main.projFrames[Projectile.type] = 8;
	}

	public override void SetDefaults()
	{
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 20;
		Projectile.timeLeft = 720;

		Projectile.penetrate = -1;
		Projectile.aiStyle = -1;
		Projectile.width = 10;
		Projectile.height = 10;

		Projectile.DamageType = DamageClass.Summon;
		Projectile.minion = true;
		Projectile.minionSlots = 1;

		ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
		ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

		Main.projPet[Projectile.type] = true;
	}

	public override bool? CanCutTiles() => false;

	public override bool MinionContactDamage() => true;

	public override void DrawBehind(
		int index,
		List<int> behindNPCsAndTiles,
		List<int> behindNPCs,
		List<int> behindProjectiles,
		List<int> overPlayers,
		List<int> overWiresUI) => behindNPCsAndTiles.Add(index);

	public override void AI()
	{
		Player owner = Main.player[Projectile.owner];

		if (!CheckPlayerActive(owner))
		{
			return;
		}

		// Manage distance to owner
		if (TeleportCooldown > 0)
		{
			TeleportCooldown--;
		}
		else if (Projectile.Center.Distance(owner.Center) > MAXDistanceToPlayer)
		{
			ResetTarget();
			TelePortTo(
				owner.MountedCenter +
				new Vector2((10 - MinionNumber * 30) * owner.direction, -40 + MathF.Sin((float)Main.timeForVisualEffects * 0.04f - MinionNumber) * 35f));
			return;
		}

		if (TargetWhoAmI == NoTarget)
		{
			GeneralBehavior();
			SearchForTargets(owner);
		}
		else
		{
			CheckTargetActive();
			Attack();
		}
	}

	/// <summary>
	/// Active check, makes sure the minion is alive while the owner is alive, and despawns if not
	/// </summary>
	private bool CheckPlayerActive(Player owner)
	{
		// Check if player is dead or not active in the game
		if (owner.dead || owner.active is not true)
		{
			owner.ClearBuff(ModContent.BuffType<AuburnBell>());

			return false;
		}

		// Check if the player has the [AuburnBell] buff
		if (owner.HasBuff(ModContent.BuffType<AuburnBell>()))
		{
			// keep the projectile alive by resetting its time left
			Projectile.timeLeft = 2;
		}

		return true;
	}

	/// <summary>
	/// Teleport to a position
	/// </summary>
	/// <param name="aim"></param>
	private void TelePortTo(Vector2 aim)
	{
		TeleportCooldown = TeleportCooldownValue;
		Projectile.Center = aim;
		for (int f = 0; f < 15; f++)
		{
			var g = Gore.NewGoreDirect(
				null,
				aim,
				new Vector2(0, Main.rand.NextFloat(10f)).RotatedByRandom(6.283),
				0,
				Main.rand.NextFloat(0.65f, Main.rand.NextFloat(2.5f, 3.75f)));
			g.timeLeft = Main.rand.Next(250, 500);
		}
	}

	/// <summary>
	/// Move to a position
	/// </summary>
	/// <param name="aim"></param>
	/// <param name="speedValue"></param>
	private void MoveTo(Vector2 aim)
	{
		float timeValue = (float)(Main.time * 0.014f);

		Projectile.velocity *= 0.97f;
		Projectile.rotation = MathF.Log(MathF.Abs(Projectile.velocity.X + 1)) * 0.2f * Projectile.direction * 0.05f + Projectile.rotation * 0.95f;
		Vector2 aimTarget = aim + new Vector2(210f * MathF.Sin(timeValue * 2 + Projectile.whoAmI) * Projectile.scale, -50f + 30f * MathF.Sin(timeValue * 0.15f + Projectile.whoAmI));
		Vector2 toAim = aimTarget - Projectile.Center - Projectile.velocity;
		if (toAim.Length() > 50)
		{
			Projectile.velocity += Vector2.Normalize(toAim) * 0.15f * Projectile.scale;
		}
	}

	/// <summary>
	/// Find the MinionAttackTargetNPC specified by player or the nearest enemy
	/// </summary>
	private void SearchForTargets(Player owner)
	{
		float minDistance = 1600f;

		Vector2 detectCenter = owner.Center;
		if (MinionNumber > 5)
		{
			detectCenter = Projectile.Center;
		}

		bool foundTarget = false;
		if (owner.HasMinionAttackTargetNPC)
		{
			NPC npc = Main.npc[owner.MinionAttackTargetNPC];

			float between = Vector2.Distance(owner.Center, detectCenter);

			if (between < minDistance)
			{
				TargetWhoAmI = npc.whoAmI;
				foundTarget = true;
			}
		}

		if (!foundTarget)
		{
			foreach (var npc in Main.ActiveNPCs)
			{
				if (npc.dontTakeDamage || npc.friendly || !npc.CanBeChasedBy() || !Collision.CanHit(Projectile, npc))
				{
					continue;
				}

				float distance = (npc.Center - detectCenter).Length();
				if (distance < minDistance)
				{
					TargetWhoAmI = npc.whoAmI;
					minDistance = distance;
					foundTarget = true;
				}
			}
		}

		Projectile.friendly = foundTarget;
	}

	/// <summary>
	/// Check if target is available
	/// </summary>
	private void CheckTargetActive()
	{
		if (TargetWhoAmI == NoTarget)
		{
			return;
		}

		NPC target = Main.npc[TargetWhoAmI];
		if (target.active && !Main.npc[TargetWhoAmI].dontTakeDamage)
		{
			return;
		}

		ResetTarget();
	}

	/// <summary>
	/// Attack logic
	/// </summary>
	private void Attack()
	{
		if (TargetWhoAmI == NoTarget)
		{
			return;
		}

		NPC target = Main.npc[TargetWhoAmI];

		// One-AttackPhase AI
		MoveTo(target.Center);
	}

	private void ResetTarget()
	{
		TargetWhoAmI = NoTarget;
	}

	/// <summary>
	/// Follow projectile's owner
	/// </summary>
	private void GeneralBehavior()
	{
		Player player = Main.player[Projectile.owner];
		Vector2 aim;

		if (player.velocity.Length() > NotMovingVelocity) // Player is moving
		{
			aim = player.MountedCenter
				+ new Vector2(
					x: (10 - MinionNumber * 30) * player.direction,
					y: -40 + MathF.Sin((float)Main.timeForVisualEffects * 0.04f - MinionNumber) * 35f);
		}
		else
		{
			aim = player.MountedCenter
				+ new Vector2(
					x: player.direction * (MathF.Sin((float)Main.timeForVisualEffects * 0.02f) * 40f - MinionNumber * 30),
					y: -50 + MathF.Sin((float)Main.timeForVisualEffects * 0.04f) * 20f);
		}

		MoveTo(aim);
		Projectile.rotation = Projectile.rotation * 0.95f + Projectile.velocity.X * 0.002f;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		int BodyLength = 12;

		SpriteBatch spriteBatch = Main.spriteBatch;

		Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

		SpriteBatchState sBS = GraphicsUtils.GetState(spriteBatch).Value;
		spriteBatch.End();
		spriteBatch.Begin(
			SpriteSortMode.Deferred,
			BlendState.NonPremultiplied,
			Main.DefaultSamplerState,
			DepthStencilState.None,
			RasterizerState.CullNone,
			null,
			Main.GameViewMatrix.TransformationMatrix);

		List<Vertex2D> bars = new List<Vertex2D>();
		Vector2 drawCenter = Projectile.Center - Main.screenPosition + new Vector2(BodyLength * 5f * 0.75f, 0).RotatedBy(Projectile.rotation) * Projectile.scale;
		for (int i = 0; i < BodyLength; i++)
		{
			Color drawColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
			float jointIndex = i / (float)BodyLength;
			int frameY = (int)(Projectile.frame + i) % Main.projFrames[Projectile.type];
			float frameYValue = frameY / (float)Main.projFrames[Projectile.type];
			float deltaYValue = 1 / (float)Main.projFrames[Projectile.type];
			float jointScale = 0.5f + 0.5f * MathF.Sin(jointIndex * MathHelper.Pi);

			bars.Add(
				drawCenter + new Vector2(-20, -20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(0, frameYValue, 0));
			bars.Add(
				drawCenter + new Vector2(-20, 20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(0, frameYValue + deltaYValue, 0));
			bars.Add(
				drawCenter + new Vector2(20, -20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(1, frameYValue, 0));
			bars.Add(
				drawCenter + new Vector2(20, -20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(1, frameYValue, 0));
			bars.Add(
				drawCenter + new Vector2(-20, 20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(0, frameYValue + deltaYValue, 0));
			bars.Add(
				drawCenter + new Vector2(20, 20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(1, frameYValue + deltaYValue, 0));

			drawCenter -= new Vector2(10, 0).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale;
		}

		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = texture;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bars.ToArray(), 0, bars.Count / 3);
		}

		spriteBatch.End();
		spriteBatch.Begin(sBS);

		// Loop frame count.
		Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];

		return false;
	}
}