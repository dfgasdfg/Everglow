using Everglow.Commons.CustomTiles.Collide;
using Everglow.Commons.CustomTiles.DataStructures;
using Everglow.Commons.CustomTiles.Tiles;

namespace Everglow.Commons.CustomTiles.EntityCollider;

public class ProjCollider : GlobalProjectile, IEntityCollider
{
	public const int HookAIStyle = 7;

	private Projectile projectile;

	public static readonly HashSet<Projectile> callFromHook = new();

	public override bool CloneNewInstances => true;

	public override bool InstancePerEntity => true;

	public override bool IsCloneable => true;

	public Direction AttachDir { get; set; }

	public RigidEntity AttachTile { get; set; }

	public AttachType AttachType { get; set; }

	public bool CanAttach => true;

	public Entity Entity => projectile;

	public Vector2 DeltaVelocity { get; set; }

	public Direction Ground => Direction.Down;

	public Vector2 Position { get; set; }

	public Vector2 AbsoluteVelocity { get; set; }

	public override GlobalProjectile Clone(Projectile from, Projectile to)
	{
		var clone = base.Clone(from, to) as ProjCollider;
		clone.projectile = to;
		clone.AttachTile = null;
		clone.AttachDir = Direction.None;
		clone.AttachType = AttachType.None;
		clone.Position = to.position;
		clone.AbsoluteVelocity = to.velocity;
		clone.DeltaVelocity = Vector2.Zero;
		return clone;
	}

	public override void Load()
	{
		On_Projectile.AI_007_GrapplingHooks += Projectile_AI_007_GrapplingHooks_On;
		On_Projectile.AI_007_GrapplingHooks_CanTileBeLatchedOnTo += On_Projectile_AI_007_GrapplingHooks_CanTileBeLatchedOnTo;
		On_Projectile.HandleMovement += Projectile_HandleMovement;
	}

	private bool On_Projectile_AI_007_GrapplingHooks_CanTileBeLatchedOnTo(On_Projectile.orig_AI_007_GrapplingHooks_CanTileBeLatchedOnTo orig, Projectile self, int x, int y)
	{
		if (callFromHook.Contains(self))
		{
			return true;
		}
		return orig(self, x, y);
	}

	private static void Projectile_AI_007_GrapplingHooks_On(On_Projectile.orig_AI_007_GrapplingHooks orig, Projectile self)
	{
		if (!TileSystem.Enable)
		{
			orig(self);
			return;
		}

		Player player = Main.player[self.owner];
		int numHooks = 3;
		if (self.type == 165)
		{
			numHooks = 8;
		}
		else if (self.type == 256)
		{
			numHooks = 2;
		}
		else if (self.type == 372)
		{
			numHooks = 2;
		}
		else if (self.type == 652)
		{
			numHooks = 1;
		}
		else if (self.type is >= 646 and <= 649)
		{
			numHooks = 4;
		}

		int grapCount = 0;
		int leastTime = int.MaxValue;
		int leastIndex = self.whoAmI;
		foreach (var proj in Main.projectile)
		{
			if (proj.active && proj.aiStyle == 7 && proj.owner == self.owner)
			{
				grapCount++;
				if (proj.timeLeft < leastTime)
				{
					leastTime = proj.timeLeft;
					leastIndex = proj.whoAmI;
				}
			}
		}
		ProjectileLoader.NumGrappleHooks(self, player, ref numHooks);
		if (grapCount > numHooks)
		{
			Main.projectile[leastIndex].Kill();
		}

		if (self.ai[0] != 1)
		{
			var collider = self.GetGlobalProjectile<ProjCollider>();
			collider.UpdateHook();
		}
		orig(self);
		callFromHook.Remove(self);
	}

	private static void Projectile_HandleMovement(On_Projectile.orig_HandleMovement orig, Projectile self, Vector2 wetVelocity, out int overrideWidth, out int overrideHeight)
	{
		if (!TileSystem.Enable || !self.tileCollide || self.aiStyle == HookAIStyle)
		{
			orig(self, wetVelocity, out overrideWidth, out overrideHeight);
			return;
		}

		TileSystem.EnableCollisionHook = false;
		var proj = self.GetGlobalProjectile<ProjCollider>();

		// 记录位置，否则会把传送当成位移
		proj.Position = self.position;
		orig(self, wetVelocity, out overrideWidth, out overrideHeight);
		IEntityCollider.Update(proj, true);
		TileSystem.EnableCollisionHook = true;
	}

	public void OnAttach()
	{
		projectile.velocity.Y = 0;
	}

	public void OnCollision(RigidEntity tile, Direction dir, ref RigidEntity newAttach)
	{
		if (dir == Direction.In)
		{
			projectile.Kill();
		}
	}

	public void OnUpdate()
	{
		if (AttachTile is not null)
		{
			projectile.position += new Vector2(0, projectile.gfxOffY);
			projectile.gfxOffY = 0;
		}
	}

	private void UpdateHook()
	{
		Player player = Main.player[projectile.owner];
		if (AttachTile is not null)
		{
			if (AttachTile.Active)
			{
				callFromHook.Add(projectile);
				(AttachTile as IHookable).SetHookPosition(projectile);
			}
			else
			{
				AttachTile = null;
			}
			return;
		}

		foreach (var tile in TileSystem.Instance.Tiles)
		{
			if (tile is IHookable hookable && tile.Collision(new CAABB(new AABB(projectile.position, projectile.Size))))
			{
				hookable.SetHookPosition(projectile);
				if (projectile.type == ProjectileID.QueenSlimeHook && projectile.alpha == 0 && Main.myPlayer == projectile.owner)
				{
					player.DoQueenSlimeHookTeleport(projectile.position);
					NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, projectile.owner);
				}
				callFromHook.Add(projectile);
				projectile.ai[0] = 2;
				projectile.netUpdate = true;
				projectile.velocity *= 0;
				if (projectile.alpha == 0)
				{
					projectile.alpha = 1;
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, projectile.Center);
				}
				AttachTile = tile;
				break;
			}
		}
	}
}