using Everglow.Myth.MiscItems.Buffs;
using Everglow.Myth.MiscItems.Dusts;
using Freeze = Everglow.Myth.MiscItems.Buffs.Freeze;

namespace Everglow.Myth.MiscItems.Projectiles.Weapon.Magic;

public class FreezeBallBrake : ModProjectile
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("FreezingMagic");
	}
	public override void SetDefaults()
	{
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 100;
		Projectile.extraUpdates = 10;
		Projectile.hostile = false;
		Projectile.tileCollide = false;
	}

	public override Color? GetAlpha(Color lightColor)
	{
		if (Projectile.timeLeft < 3584)
		{
			return new Color?(new Color(255, 255, 255, 0));
		}
		else
		{
			return new Color?(new Color((3600 - Projectile.timeLeft) / 14f, (3600 - Projectile.timeLeft) / 14f, (3600 - Projectile.timeLeft) / 14f, 0));
		}
	}
	private bool Ini = false;
	private int Jnj = 15;
	private int Jnj2 = 6;
	public override void AI()
	{
		Player p = Main.player[Projectile.owner];
		Jnj--;
		Jnj2--;
		if (Projectile.timeLeft > 1 && Projectile.timeLeft < 100)
		{
			if (Projectile.timeLeft % 1 == 0)
			{
				if (Projectile.timeLeft / 25f > 5)
				{
					int r = Dust.NewDust(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<Ice>(), 0, 0, 0, default, 5);
					Main.dust[r].velocity.X = 0;
					Main.dust[r].velocity.Y = 0;
					Main.dust[r].noGravity = true;
					Main.dust[r].rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
					if (Jnj > 0)
					{
						Main.dust[r].alpha = Jnj * 16;
					}
					else
					{
						Main.dust[r].alpha = 60;
					}
				}
				else
				{
					int r = Dust.NewDust(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<Ice>(), 0, 0, 0, default, Projectile.timeLeft / 25f);
					Main.dust[r].velocity.X = 0;
					Main.dust[r].velocity.Y = 0;
					Main.dust[r].noGravity = true;
					Main.dust[r].rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
					if (Jnj > 0)
					{
						Main.dust[r].alpha = Jnj * 40;
					}
					else
					{
						Main.dust[r].alpha = 60;
					}
				}
			}
			Projectile.velocity *= 1 - 0.3f / Projectile.timeLeft;
		}
		if (Jnj2 > 0)
		{
			int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 0, 0, ModContent.DustType<Dusts.Freeze>(), 0, 0, 0, default, 10f);
			Main.dust[r].noGravity = true;
		}
		Projectile.tileCollide = false;
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		if (!target.HasBuff(ModContent.BuffType<Freeze>()))
		{
			target.AddBuff(ModContent.BuffType<Freeze>(), (int)Projectile.ai[1]);
			target.AddBuff(ModContent.BuffType<Freeze2>(), (int)Projectile.ai[1] + 2);
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (target.type != NPCID.MoonLordHead && target.type != NPCID.MoonLordHand && target.type != NPCID.MoonLordCore)
		{
			if (!target.HasBuff(ModContent.BuffType<Freeze>()))
			{
				target.AddBuff(ModContent.BuffType<Freeze>(), (int)Projectile.ai[1]);
				target.AddBuff(ModContent.BuffType<Freeze2>(), (int)Projectile.ai[1] + 2);
			}
		}
		if (target.type == NPCID.WallofFlesh)
		{
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].type == NPCID.WallofFlesh || Main.npc[i].type == NPCID.WallofFleshEye)
				{
					if (!target.HasBuff(ModContent.BuffType<Freeze>()))
					{
						target.AddBuff(ModContent.BuffType<Freeze>(), (int)Projectile.ai[1]);
						target.AddBuff(ModContent.BuffType<Freeze2>(), (int)Projectile.ai[1] + 2);
					}
				}
			}
		}
		if (target.type == NPCID.WallofFleshEye)
		{
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].type == NPCID.WallofFlesh || Main.npc[i].type == NPCID.WallofFleshEye)
				{
					if (!target.HasBuff(ModContent.BuffType<Freeze>()))
					{
						target.AddBuff(ModContent.BuffType<Freeze>(), (int)Projectile.ai[1]);
						target.AddBuff(ModContent.BuffType<Freeze2>(), (int)Projectile.ai[1] + 2);
					}
				}
			}
		}
	}
}