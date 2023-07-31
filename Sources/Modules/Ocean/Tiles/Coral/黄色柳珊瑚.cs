﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace Everglow.Ocean.Tiles.Ocean
{
	// Token: 0x02000DD9 RID: 3545
	public class 黄色柳珊瑚 : ModTile
	{
		// Token: 0x0600489C RID: 18588 RVA: 0x003496D4 File Offset: 0x003478D4
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[(int)base.Type] = true;
			Main.tileNoAttach[(int)base.Type] = true;
            this.MinPick = 300;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
                16,
                16,
                16,
                20
			};
            TileObjectData.newTile.CoordinateWidth = 96;
            TileObjectData.addTile((int)base.Type);
			this.DustType = -1;
            LocalizedText modTranslation = base.CreateMapEntryName();
            base.AddMapEntry(new Color(229, 165, 0), modTranslation);
            // modTranslation.SetDefault("");
            this.MineResist = 3f;
			base.SetStaticDefaults();
			// modTranslation.AddTranslation(GameCulture.Chinese, "");
		}
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 16, 32, ModContent.ItemType<Everglow.Ocean.Items.YellowGorgonian>());
        }
        // Token: 0x0600489D RID: 18589 RVA: 0x000138D5 File Offset: 0x00011AD5
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = (fail ? 1 : 3);
		}
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 4));
            Main.tile[i, j].TileFrameX = (short)(num * 96);
            Main.tile[i, j + 2].TileFrameX = (short)(num * 96);
            Main.tile[i, j + 1].TileFrameX = (short)(num * 96);
            Main.tile[i, j + 4].TileFrameX = (short)(num * 96);
            Main.tile[i, j + 5].TileFrameX = (short)(num * 96);
            Main.tile[i, j + 3].TileFrameX = (short)(num * 96);
        }
    }
}
