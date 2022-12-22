﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Everglow.Resources.NPCList.EventNPCs;
using Everglow.Sources.Modules.FoodModule.Buffs;
using Everglow.Sources.Modules.FoodModule.Buffs.ModDrinkBuffs;
using Everglow.Sources.Modules.FoodModule.Buffs.VanillaFoodBuffs;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles;
using Everglow.Sources.Modules.MythModule;
using Steamworks;
using Terraria.DataStructures;
using Terraria.Audio;

namespace Everglow.Sources.Modules.FoodModule.Buffs
{
    public class FoodBuffGlobalNPC : GlobalNPC
    {
        public bool isservant = false;
        public override bool InstancePerEntity => true;
        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            Player player = Main.LocalPlayer;
            if (crit)
            {
                damage *= (FoodBuffModPlayer.CritDamage + 1) / 2f;
            }
            if (player != null && player.active && !player.dead)
            {
                if (Main.bloodMoon && BloodMoonNPCs.vanillaBloodMoonNPCs.Contains(npc.type) && player.GetModPlayer<FoodBuffModPlayer>().PurpleHooterBuff)
                {
                    damage *= 5;
                }
                if (Main.snowMoon && FrostMoonNPCs.vanillaFrostMoonNPCs.Contains(npc.type) && player.GetModPlayer<FoodBuffModPlayer>().RedWineBuff)
                {
                    damage *= 5;
                }
                if (Main.pumpkinMoon && PumpkinMoonNPCs.vanillaPumpkinMoonNPCs.Contains(npc.type) && player.GetModPlayer<FoodBuffModPlayer>().QuinceMarryBuff)
                {
                    damage *= 5;
                }
                if (Main.eclipse && EclipseNPCs.vanillaEclipseNPCs.Contains(npc.type) && player.GetModPlayer<FoodBuffModPlayer>().SunriseBuff)
                {
                    damage *= 5;
                }
                if (npc.GetGlobalNPC<FoodBuffGlobalNPC>().isservant && player.GetModPlayer<FoodBuffModPlayer>().TricolourBuff)
                {
                    damage *= 5;
                }
                if (player.GetModPlayer<FoodBuffModPlayer>().StrawberryBuff)
                {
                    damage  =  Math.Clamp(Math.Log10((double)1 / ((npc.Center - player.Center).Length() / 1000) + 10), 1, 1.25f)* damage;
                }
                if (player.GetModPlayer<FoodBuffModPlayer>().StrawberryIcecreamBuff)
                {
                    damage = Math.Clamp(Math.Log((double)1 / ((npc.Center - player.Center).Length() / 100) + 2.5), 1, 1.33f) * damage;
                }
                /*if (灯笼月 && PiercoldWindBuff)
             {
                 damage *= 5;
             }*/
            }

            return base.StrikeNPC(npc, ref damage, defense, ref knockback, hitDirection, ref crit);
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (source is EntitySource_Parent)
            {
                EntitySource_Parent parentSource = source as EntitySource_Parent;
                if (parentSource.Entity is NPC && (parentSource.Entity as NPC).boss && !npc.boss)
                {
                    isservant = true;
                }
            }
        }
        public override void OnKill(NPC npc)
        {
            if (npc.HasBuff(ModContent.BuffType<CherryBuff>()))
            {
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, npc.Center);
                Projectile.NewProjectile(npc.GetSource_Death("CherryBuff"), npc.Center, Vector2.Zero, ModContent.ProjectileType<BombShakeWave>(), 0, 0, npc.whoAmI, 0.4f, 2f);
                float k1 = Math.Clamp(npc.velocity.Length(), 1, 3);
                float k2 = Math.Clamp(npc.velocity.Length(), 6, 10);
                float k0 = 1f / 4 * k2;
                for (int j = 0; j < 8 * k0; j++)
                {
                    Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * k1;
                    int dust0 = Dust.NewDust(npc.Center, 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), v0.X / 10, v0.Y / 10, 100, default, Main.rand.NextFloat(0.6f, 1.8f) * 2);
                    Main.dust[dust0].noGravity = true;
                }
                for (int j = 0; j < 16 * k0; j++)
                {
                    Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * k1;
                    int dust1 = Dust.NewDust(npc.Center, 0, 0, ModContent.DustType<BlueParticleDark2StoppedByTile>(), v0.X / 10, v0.Y / 10, 100, default, Main.rand.NextFloat(3.7f, 5.1f) * 2);
                    Main.dust[dust1].alpha = (int)(Main.dust[dust1].scale * 50 / k0);
                    Main.dust[dust1].rotation = Main.rand.NextFloat(0, 6.283f);
                }
                for (int j = 0; j < 16 * k0; j++)
                {
                    Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * k1;
                    int dust1 = Dust.NewDust(npc.Center, 0, 0, ModContent.DustType<MothSmog>(), v0.X / 10, v0.Y / 10, 100, default, Main.rand.NextFloat(3.7f, 5.1f) * 2);
                    Main.dust[dust1].alpha = (int)(Main.dust[dust1].scale * 50 / k0);
                    Main.dust[dust1].rotation = Main.rand.NextFloat(0, 6.283f);
                }
                foreach (NPC target in Main.npc)
                {
                    if (target != npc)
                    {
                        float Dis = (target.Center - npc.Center).Length();
                        if (Dis < 250)
                        {
                            if (!target.dontTakeDamage && !target.friendly && target.active)
                            {
                                target.AddBuff(ModContent.BuffType<CherryBuff>(), 1200);
                                target.StrikeNPC(Main.rand.Next(80, 160) - target.defense, Main.rand.Next(8, 24), 0, Main.rand.NextBool(22, 33));
                            }
                        }
                    }
                }
                CombatText.NewText(npc.Hitbox, Color.HotPink, "可汗，再带我冲一次吧"); //TODO localization
                npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<CherryBuff>()));
            }
        }
    }
}
