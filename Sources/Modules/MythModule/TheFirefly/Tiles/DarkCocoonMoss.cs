using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class DarkCocoonMoss : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<DarkCocoon>()] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMoss[Type] = true;
            MinPick = 175;
            DustType = 191;
            ItemDrop = ModContent.ItemType<Items.DarkCocoon>();
            AddMapEntry(new Color(35, 49, 122));
        }

        private void PlantTree(int i, int j, int style)
        {
            for (int y = -8; y < 0; y++)
            {
                Tile tile = Main.tile[i, j + y];
                tile.TileType = (ushort)ModContent.TileType<Tiles.FireflyTree>();
                tile.HasTile = true;
                tile.TileFrameX = (short)(style * 256);
                tile.TileFrameY = (short)((y + 8) * 16);
            }
            var fireFlyTree = ModContent.GetInstance<FireflyTree>();
            fireFlyTree.InsertOneTreeRope(i, j - 8, style);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            RandomUpdate(i, j);//TODO:为了让这玩意效果正常强行采取的暴力措施，如果sublib更新了就删掉

            base.NearbyEffects(i, j, closer);
        }

        public override void RandomUpdate(int i, int j)
        {
            if (Main.rand.NextBool(2))//黑萤苔藓
            {
                Tile t0 = Main.tile[i, j];
                Tile t1 = Main.tile[i, j - 1];
                Tile t2 = Main.tile[i, j + 1];
                Tile t3 = Main.tile[i + 1, j];
                Tile t4 = Main.tile[i - 1, j];
                if (t0.Slope == SlopeType.Solid && !t2.HasTile)
                {
                    t2.TileType = (ushort)ModContent.TileType<Tiles.FireflyMoss>();
                    t2.HasTile = true;
                    t2.TileFrameY = (short)(Main.rand.Next(3, 6) * 18);
                }
                if (t0.Slope == SlopeType.Solid && !t4.HasTile)
                {
                    t4.TileType = (ushort)ModContent.TileType<Tiles.FireflyMoss>();
                    t4.HasTile = true;
                    t4.TileFrameY = (short)(Main.rand.Next(9, 12) * 18);
                }
                if (t0.Slope== SlopeType.Solid && !t3.HasTile)
                {
                    t3.TileType = (ushort)ModContent.TileType<Tiles.FireflyMoss>();
                    t3.HasTile = true;
                    t3.TileFrameY = (short)(Main.rand.Next(6, 9) * 18);
                }
                if (t0.Slope == SlopeType.Solid && !t1.HasTile)
                {
                    t1.TileType = (ushort)ModContent.TileType<Tiles.FireflyMoss>();
                    t1.HasTile = true;
                    t1.TileFrameY = (short)(Main.rand.Next(0, 3) * 18);
                }
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            var tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/DarkCocoonMossGlow");

            spriteBatch.Draw(tex, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(255, 255, 255, 0), 0, new Vector2(0), 1, SpriteEffects.None, 0);

            base.PostDraw(i, j, spriteBatch);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}