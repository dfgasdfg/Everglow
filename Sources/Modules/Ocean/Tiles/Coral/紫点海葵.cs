using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
	public class 紫点海葵 : ModTile
	{
        private float num = 0;
        private int num2 = 0;
        private bool num3 = false;
        private bool flag2 = false;
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            this.RegisterItemDrop(ModContent.ItemType<Everglow.Ocean.Items.PurpleSeaAnemone>());
            TileObjectData.addTile((int)base.Type);
            LocalizedText modTranslation = base.CreateMapEntryName();
            // modTranslation.SetDefault("");
            base.AddMapEntry(new Color(110, 87, 195), modTranslation);
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			return true;
		}
        public override void NearbyEffects(int i, int j, bool closer)
        {
            bool flag = false;
            Player player = Main.player[Main.myPlayer];
            if((player.Center - new Vector2(i * 16, j * 16)).Length() < 70)
            {
                flag = true;
            }

            if(flag)
            {
                num += (3 / (MythWorld.紫点海葵 + 0.0001f)) * Main.rand.Next(1000, 1200) / 1000f;
                if (i % 2 == 1)
                {
                    if ((int)num % (3) == 0 && Main.tile[i, j].TileFrameY < 72)
                    {
                        Main.tile[i, j].TileFrameY += 18;
                    }
                }
                if (i % 2 == 0)
                {
                    if ((int)num % (3) == 0 && Main.tile[i, j].TileFrameY < 72)
                    {
                        Main.tile[i, j].TileFrameY += 18;
                    }
                }
                flag2 = true;
            }
            if (Main.tile[i, j].TileFrameY >= 18 && !flag)
            {
                num += 3 * Main.rand.Next(1000, 1200) / 1000f;
                if (i % 2 == 1)
                {
                    if ((int)num % (5) == 0 && Main.tile[i, j].TileFrameY > 18)
                    {
                        Main.tile[i, j].TileFrameY -= 18;
                    }
                }
                if (i % 2 == 0)
                {
                    if ((int)num % (5) == 0 && Main.tile[i, j].TileFrameY > 18)
                    {
                        Main.tile[i, j].TileFrameY -= 18;
                    }
                }
                if (Main.tile[i, j].TileFrameY < 18)
                {
                    flag2 = false;
                }
            }
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 3) * 18);
            Main.tile[i, j].TileFrameX = num;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = 16;
            Main.spriteBatch.Draw(ModAsset.紫点海葵Glow.Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(55,55,55,0), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
