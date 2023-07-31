﻿using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Everglow.Ocean.NPCs
{
	// Token: 0x020004FC RID: 1276
    public class SuperWaterBomb : ModNPC
	{
		// Token: 0x06001BA4 RID: 7076 RVA: 0x0000B6E0 File Offset: 0x000098E0
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("SuperWaterBomb");
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "超级水雷");
			Main.npcFrameCount[base.NPC.type] = 1;
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x001539E4 File Offset: 0x00151BE4
		public override void SetDefaults()
		{
			base.NPC.damage = 1000;
			base.NPC.width = 212;
			base.NPC.height = 210;
			base.NPC.defense = 0;
            base.NPC.lifeMax = 1;
			base.NPC.knockBackResist = 0f;
			base.NPC.alpha = 0;
			base.NPC.noGravity = true;
			base.NPC.noTileCollide = true;
		}
        private int u = 0;

		public override void AI()
		{
            base.NPC.spriteDirection = -1;
            u += 1;
            base.NPC.velocity.Y = (float)Math.Sin((float)u / 105f * Math.PI);
        }

		public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (projectile.penetrate == -1)
			{
				damage = (int)((double)damage * 2);
				return;
			}
			if (projectile.penetrate > 1)
			{
				damage = (int)((double)damage * 4);
				return;
			}
			projectile.penetrate = 1;
		}
        // Token: 0x02000413 RID: 1043
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/烟花爆炸"), (int)NPC.Center.X, (int)NPC.Center.Y);
            player.velocity = (NPC.velocity - player.velocity) / (NPC.velocity - player.velocity).Length() * 54;
            Projectile.NewProjectile(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f, 0f, 0f, 164, 10, 4f, Main.myPlayer, 0f, 0f);
            for (int k = 0; k <= 10; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(16,160)).RotatedByRandom(Math.PI * 2);
                Gore.NewGore(base.NPC.Center, v, base.Mod.GetGoreSlot("Gores/超级水雷碎块"), 1f);
            }
            for (int k = 0; k <= 10; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(16, 160)).RotatedByRandom(Math.PI * 2);
                Gore.NewGore(base.NPC.Center, v, base.Mod.GetGoreSlot("Gores/超级水雷碎块2"), 1f);
            }
            for (int k = 0; k <= 30; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(0, 140)).RotatedByRandom(Math.PI * 2);
                int num4 = Projectile.NewProjectile(NPC.Center.X + v.X, NPC.Center.Y + v.Y, 0, 0,ModContent.ProjectileType<Everglow.Ocean.Projectiles.熔炉烈焰>(), 1000, 0, Main.myPlayer, Main.rand.Next(1000, 3000) / 700f, 0f);
                Main.projectile[num4].hostile = true;
            }
        }
        // Token: 0x02000413 RID: 1043
        public override void HitEffect(NPC.HitInfo hit)
        {
            Player player = Main.player[Main.myPlayer];
            SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/烟花爆炸"), (int)NPC.Center.X, (int)NPC.Center.Y);
            Projectile.NewProjectile(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f, 0f, 0f, 164, 10, 4f, Main.myPlayer, 0f, 0f);
            for (int k = 0; k <= 10; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(16,160)).RotatedByRandom(Math.PI * 2);
                Gore.NewGore(base.NPC.position, v, base.Mod.GetGoreSlot("Gores/超级水雷碎块"), 1f);
            }
            for (int k = 0; k <= 10; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(16,160)).RotatedByRandom(Math.PI * 2);
                Gore.NewGore(base.NPC.position, v, base.Mod.GetGoreSlot("Gores/超级水雷碎块2"), 1f);
            }
            for (int k = 0; k <= 30; k++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(0, 140)).RotatedByRandom(Math.PI * 2);
                int num4 = Projectile.NewProjectile(NPC.Center.X + v.X, NPC.Center.Y + v.Y, 0, 0,ModContent.ProjectileType<Everglow.Ocean.Projectiles.熔炉烈焰>(), 1000, 0, Main.myPlayer, Main.rand.Next(1000, 3000) / 700f, 0f);
                Main.projectile[num4].hostile = true;
            }
        }
		// Token: 0x06001BA8 RID: 7080 RVA: 0x000037AF File Offset: 0x000019AF
		public override bool CheckActive()
		{
			return false;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe)
			{
				return 0f;
			}
			if (spawnInfo.Player.GetModPlayer<OceanContentPlayer>().ZoneOcean && spawnInfo.Water)
			{
				return 0.01f;
			}
			else
            {
                return 0f;
            }
		}
		// Token: 0x06001BA9 RID: 7081 RVA: 0x000037AF File Offset: 0x000019AF
		public override bool PreKill()
		{
			return false;
		}
	}
}
