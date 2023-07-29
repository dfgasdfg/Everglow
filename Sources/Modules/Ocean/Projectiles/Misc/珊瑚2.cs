﻿using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Everglow.Ocean.MiscImplementation;
using Terraria;
using Terraria.GameContent.Generation;
using Everglow.Ocean.Tiles;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Everglow.Ocean.Projectiles.Ocean
{
	// Token: 0x0200054E RID: 1358
    public class 珊瑚2 : ModProjectile
	{
		// Token: 0x06001DC0 RID: 7616 RVA: 0x0000C2EA File Offset: 0x0000A4EA
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("珊瑚");
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x0017F224 File Offset: 0x0017D424
		public override void SetDefaults()
		{
			base.Projectile.width = 2;
			base.Projectile.height = 2;
			base.Projectile.friendly = true;
			base.Projectile.alpha = 255;
			base.Projectile.timeLeft = 600;
			base.Projectile.penetrate = 1;
            Projectile.extraUpdates = (int)2f;
			base.Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x0017F28C File Offset: 0x0017D48C
		public override void AI()
		{
            Projectile.velocity.Y += 0.15f;
        }
		// Token: 0x06001DC3 RID: 7619 RVA: 0x0017F570 File Offset: 0x0017D770
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(70, 600);
            target.AddBuff(69, 600);
        }
        public override void Kill(int timeLeft)
        {
            if (Main.tile[(int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 + 1].TileType == 396 && Main.tile[(int)Projectile.position.X / 16 + 2, (int)Projectile.position.Y / 16 + 1].TileType == 396 && Main.tile[(int)Projectile.position.X / 16 + 2, (int)Projectile.position.Y / 16 - 1].TileType <= 396 && Main.tile[(int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 - 1].TileType <= 396)
            {
                WorldGen.PlaceTile((int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 - 2, (ushort)Mod.Find<ModTile>("巨大鹿角珊瑚").Type, true, false, -1, 0);
            }
        }
    }
}
