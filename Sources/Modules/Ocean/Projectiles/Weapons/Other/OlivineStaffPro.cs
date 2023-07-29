﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles.projectile2
{
    public class OlivineStaffPro : ModProjectile
	{
		public override void SetDefaults()
		{
			base.Projectile.width = 16;
			base.Projectile.height = 16;
			base.Projectile.aiStyle = 1;
			base.Projectile.alpha = 255;
			base.Projectile.scale = 1f;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = DamageClass.Magic;
			base.Projectile.penetrate = 2;
			base.Projectile.timeLeft = 3600;
			this.AIType = 14;
		}
		public override void AI()
		{
			if(base.Projectile.timeLeft <= 3594)
			{
                int num = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y), 10, 10, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), 0f, 0f, 100, default(Color), 1.5f);
                int num1 = Dust.NewDust(base.Projectile.Center, 10, 10, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), (float)Main.rand.Next(-130, 130) / 100f, (float)Main.rand.Next(-130, 130) / 100f, 0, default(Color), 1.5f);
                int num2 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y), 10, 10, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), 0f, 0f, 100, default(Color), 1.5f);
                Dust dust = Main.dust[num];
                dust = Main.dust[num];
                dust = Main.dust[num1];
                dust = Main.dust[num2];
                dust.velocity *= 0.04f;
                Main.dust[num].noGravity = true;
                Main.dust[num1].noGravity = true;
                Main.dust[num2].noGravity = true;
            }
            float num20 = base.Projectile.Center.X;
            float num30 = base.Projectile.Center.Y;
            float num4 = 400f;
            bool flag = false;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(base.Projectile, false) && Collision.CanHit(base.Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(base.Projectile.position.X + (float)(base.Projectile.width / 2) - num5) + Math.Abs(base.Projectile.position.Y + (float)(base.Projectile.height / 2) - num6);
                    if (num7 < num4)
                    {
                        num4 = num7;
                        num20 = num5;
                        num30 = num6;
                        flag = true;
                    }
                    if (num7 < 50)
                    {
                        Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), Projectile.knockBack, Projectile.direction, Main.rand.Next(200) > 150 ? true : false);
                        Projectile.penetrate--;
                    }
                }
            }
            if (flag)
            {
                float num8 = 20f;
                Vector2 vector1 = new Vector2(base.Projectile.position.X + (float)base.Projectile.width * 0.5f, base.Projectile.position.Y + (float)base.Projectile.height * 0.5f);
                float num9 = num20 - vector1.X;
                float num10 = num30 - vector1.Y;
                float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                num11 = num8 / num11;
                num9 *= num11;
                num10 *= num11;
                base.Projectile.velocity.X = (base.Projectile.velocity.X * 40f + num9) / 41f;
                base.Projectile.velocity.Y = (base.Projectile.velocity.Y * 40f + num10) / 41f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(Projectile.penetrate >= 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    int num = Dust.NewDust(base.Projectile.Center, base.Projectile.width, base.Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), base.Projectile.oldVelocity.X, base.Projectile.oldVelocity.Y, 0, default(Color), 2.7f);
                    int num1 = Dust.NewDust(base.Projectile.Center, base.Projectile.width, base.Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), base.Projectile.oldVelocity.X, base.Projectile.oldVelocity.Y, 0, default(Color), 2.6f);
                    int num2 = Dust.NewDust(base.Projectile.Center, base.Projectile.width, base.Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), base.Projectile.oldVelocity.X, base.Projectile.oldVelocity.Y, 0, default(Color), 2.2f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num1].noGravity = true;
                    Main.dust[num2].noGravity = true;
                    Vector2 v = Projectile.oldVelocity.RotatedBy(Main.rand.NextFloat(-0.8f, 0.8f)) * Main.rand.NextFloat(0.2f, 0.8f) * i;
                    Projectile.NewProjectile(Projectile.Center.X + v.X, Projectile.Center.Y + v.Y, 0, 0, ModContent.ProjectileType<Everglow.Ocean.Projectiles.OlivineStaffPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, i * 9);
                }
            }
        }
        public override void Kill(int timeLeft)
		{
            if (Projectile.penetrate < 1)
            {
                for (int i = 0; i < 20; i++)
                {
                    int num = Dust.NewDust(base.Projectile.Center, base.Projectile.width, base.Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), base.Projectile.oldVelocity.X, base.Projectile.oldVelocity.Y, 0, default(Color), 2.7f);
                    int num1 = Dust.NewDust(base.Projectile.Center, base.Projectile.width, base.Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), base.Projectile.oldVelocity.X, base.Projectile.oldVelocity.Y, 0, default(Color), 2.6f);
                    int num2 = Dust.NewDust(base.Projectile.Center, base.Projectile.width, base.Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), base.Projectile.oldVelocity.X, base.Projectile.oldVelocity.Y, 0, default(Color), 2.2f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num1].noGravity = true;
                    Main.dust[num2].noGravity = true;
                    Vector2 v = Projectile.oldVelocity.RotatedBy(Main.rand.NextFloat(-0.8f, 0.8f)) * Main.rand.NextFloat(0.2f, 2.4f) * i;
                    Projectile.NewProjectile(Projectile.Center.X + v.X, Projectile.Center.Y + v.Y, 0, 0, ModContent.ProjectileType<Everglow.Ocean.Projectiles.OlivineStaffPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, i * 4.5f);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    int num = Dust.NewDust(base.Projectile.Center, base.Projectile.width, base.Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), base.Projectile.oldVelocity.X, base.Projectile.oldVelocity.Y, 0, default(Color), 2.7f);
                    int num1 = Dust.NewDust(base.Projectile.Center, base.Projectile.width, base.Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), base.Projectile.oldVelocity.X, base.Projectile.oldVelocity.Y, 0, default(Color), 2.6f);
                    int num2 = Dust.NewDust(base.Projectile.Center, base.Projectile.width, base.Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), base.Projectile.oldVelocity.X, base.Projectile.oldVelocity.Y, 0, default(Color), 2.2f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num1].noGravity = true;
                    Main.dust[num2].noGravity = true;
                    Vector2 v = Projectile.oldVelocity.RotatedBy(Main.rand.NextFloat(-0.8f, 0.8f)) * Main.rand.NextFloat(0.2f, 0.8f) * i;
                    Projectile.NewProjectile(Projectile.Center.X + v.X, Projectile.Center.Y + v.Y, 0, 0, ModContent.ProjectileType<Everglow.Ocean.Projectiles.OlivineStaffPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, i * 9);
                }
            }
		}
	}
}
