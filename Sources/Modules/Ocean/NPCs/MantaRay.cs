using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Everglow.Ocean.NPCs
{
    public class MantaRay : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("Sailfish");
			Main.npcFrameCount[base.NPC.type] = 7;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "斑点魟");
		}
		public override void SetDefaults()
		{
			base.NPC.noGravity = true;
			base.NPC.damage = 140;
			base.NPC.width = 100;
			base.NPC.height = 68;
			base.NPC.defense = 25;
			base.NPC.lifeMax = 1400;
			base.NPC.aiStyle = 16;
			this.AIType = -1;
			for (int i = 0; i < base.NPC.buffImmune.Length; i++)
			{
				base.NPC.buffImmune[i] = true;
			}
			base.NPC.value = (float)Item.buyPrice(0, 1, 6, 0);
			base.NPC.HitSound = SoundID.NPCHit1;
			base.NPC.DeathSound = SoundID.NPCDeath40;
			base.NPC.knockBackResist = 0.2f;
            //this.banner = base.npc.type;
            //this.bannerItem = ModContent.ItemType<Everglow.Ocean.Items.斑点魟Banner>();
        }
		public override void AI()
		{
			base.NPC.spriteDirection = ((base.NPC.direction > 0) ? 1 : -1);
			base.NPC.noGravity = true;
			base.NPC.chaseable = this.hasBeenHit;
			if (base.NPC.direction == 0)
			{
				base.NPC.TargetClosest(true);
			}
			if (base.NPC.justHit)
			{
				this.hasBeenHit = true;
			}
			if (base.NPC.wet)
			{
				bool flag = this.hasBeenHit;
				base.NPC.TargetClosest(false);
				if (Main.player[base.NPC.target].wet && !Main.player[base.NPC.target].dead && (Main.player[base.NPC.target].Center - base.NPC.Center).Length() < 200f)
				{
					flag = true;
				}
				if (Main.player[base.NPC.target].dead && flag)
				{
					flag = false;
				}
				if (!flag)
				{
					if (base.NPC.collideX)
					{
						base.NPC.velocity.X = base.NPC.velocity.X * -10f;
						base.NPC.direction *= -1;
						base.NPC.netUpdate = true;
					}
					if (base.NPC.collideY)
					{
						base.NPC.netUpdate = true;
						if (base.NPC.velocity.Y > 0f)
						{
							base.NPC.velocity.Y = Math.Abs(base.NPC.velocity.Y) * -1f;
							base.NPC.directionY = -1;
							base.NPC.ai[0] = -1f;
						}
						else if (base.NPC.velocity.Y < 0f)
						{
							base.NPC.velocity.Y = Math.Abs(base.NPC.velocity.Y);
							base.NPC.directionY = 1;
							base.NPC.ai[0] = 1f;
						}
					}
				}
				if (flag)
				{
					base.NPC.TargetClosest(true);
					base.NPC.velocity.X = base.NPC.velocity.X + (float)base.NPC.direction * 0.5f;
					base.NPC.velocity.Y = base.NPC.velocity.Y + (float)base.NPC.directionY * 0.1f;
					if (base.NPC.velocity.X > 11f)
					{
						base.NPC.velocity.X = 11f;
					}
					if (base.NPC.velocity.X < -11f)
					{
						base.NPC.velocity.X = -11f;
					}
					if (base.NPC.velocity.Y > 8f)
					{
						base.NPC.velocity.Y = 8f;
					}
					if (base.NPC.velocity.Y < -8f)
					{
						base.NPC.velocity.Y = -8f;
					}
				}
				else
				{
					base.NPC.velocity.X = base.NPC.velocity.X + (float)base.NPC.direction * 0.1f;
					if (base.NPC.velocity.X < -6.5f || base.NPC.velocity.X > 6.5f)
					{
						base.NPC.velocity.X = base.NPC.velocity.X * 0.95f;
					}
					if (base.NPC.ai[0] == -1f)
					{
						base.NPC.velocity.Y = base.NPC.velocity.Y - 0.01f;
						if ((double)base.NPC.velocity.Y < -0.3)
						{
							base.NPC.ai[0] = 1f;
						}
					}
					else
					{
						base.NPC.velocity.Y = base.NPC.velocity.Y + 0.01f;
						if ((double)base.NPC.velocity.Y > 1.5f)
						{
							base.NPC.ai[0] = -1f;
						}
					}
				}
				int num = (int)(base.NPC.position.X + (float)(base.NPC.width / 2)) / 16;
				int num2 = (int)(base.NPC.position.Y + (float)(base.NPC.height / 2)) / 16;
				if (Main.tile[num, num2 - 1] == null)
				{
					Main.tile[num, num2 - 1] = new Tile();
				}
				if (Main.tile[num, num2 + 1] == null)
				{
					Main.tile[num, num2 + 1] = new Tile();
				}
				if (Main.tile[num, num2 + 2] == null)
				{
					Main.tile[num, num2 + 2] = new Tile();
				}
				if (Main.tile[num, num2 - 1].LiquidAmount > 128)
				{
					if (Main.tile[num, num2 + 1].HasTile)
					{
						base.NPC.ai[0] = -1f;
					}
					else if (Main.tile[num, num2 + 2].HasTile)
					{
						base.NPC.ai[0] = -1f;
					}
				}
				if ((double)base.NPC.velocity.Y > 0.4 || (double)base.NPC.velocity.Y < -0.4)
				{
					base.NPC.velocity.Y = base.NPC.velocity.Y * 0.95f;
				}
			}
			else
			{
				if (base.NPC.velocity.Y == 0f)
				{
					base.NPC.velocity.X = base.NPC.velocity.X * 0.94f;
					if ((double)base.NPC.velocity.X > -0.2 && (double)base.NPC.velocity.X < 0.2)
					{
						base.NPC.velocity.X = 0f;
					}
				}
				base.NPC.velocity.Y = base.NPC.velocity.Y + 0.4f;
				if (base.NPC.velocity.Y > 12f)
				{
					base.NPC.velocity.Y = 12f;
				}
				base.NPC.ai[0] = 1f;
			}
			base.NPC.rotation = base.NPC.velocity.Y * (float)base.NPC.direction * 0.1f;
			if ((double)base.NPC.rotation < -0.2)
			{
				base.NPC.rotation = -0.2f;
			}
			if ((double)base.NPC.rotation > 0.2)
			{
				base.NPC.rotation = 0.2f;
			}
		}
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if (projectile.minion)
			{
				return new bool?(this.hasBeenHit);
			}
			return null;
		}
		public override void FindFrame(int frameHeight)
		{
			base.NPC.frameCounter += (double)(this.hasBeenHit ? 0.15f : 0.075f);
			base.NPC.frameCounter %= (double)Main.npcFrameCount[base.NPC.type];
			int num = (int)base.NPC.frameCounter;
			base.NPC.frame.Y = num * frameHeight;
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe)
			{
				return 0f;
			}
			if (spawnInfo.Player.GetModPlayer<OceanContentPlayer>().ZoneOcean && spawnInfo.Water)
			{
				return 0.1f;
			}
			return 0f;
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			target.AddBuff(30, 420, true);
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 5, hit.HitDirection, -1f, 0, default(Color), 1f);
			}
			if (base.NPC.life <= 0)
			{
				for (int j = 0; j < 25; j++)
				{
					Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 5, hit.HitDirection, -1f, 0, default(Color), 1f);
				}
                float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, ModContent.Find<ModGore>("Everglow/斑点魟碎块").Type, 1f);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, ModContent.Find<ModGore>("Everglow/斑点魟碎块2").Type, 1f);
			}
			base.NPC.spriteDirection = ((base.NPC.direction > 0) ? 1 : -1);
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (base.NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 value = new Vector2(base.NPC.Center.X, base.NPC.Center.Y);
            Vector2 vector = new Vector2((float)(TextureAssets.Npc[base.NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[base.NPC.type].Value.Height / Main.npcFrameCount[base.NPC.type] / 2));
            Vector2 vector2 = value - Main.screenPosition;
            vector2 -= new Vector2((float)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/斑点魟发光部分").Width(), (float)(ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/斑点魟发光部分").Height() / Main.npcFrameCount[base.NPC.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.NPC.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(97 - base.NPC.alpha, 97 - base.NPC.alpha, 97 - base.NPC.alpha, 0), Color.White);
            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/斑点魟发光部分"), vector2, new Rectangle?(base.NPC.frame), color, base.NPC.rotation, vector, 1f, effects, 0f);
        }
		public bool hasBeenHit;
		public override void OnKill()
        {
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem(NPC.GetSource_Death(), (int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.OceanDustCore>(), 1, false, 0, false, false);
            }
        }
	}
}
