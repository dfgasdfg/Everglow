﻿using Terraria.ObjectData;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Tiles.CyanVine
{
	public class CyanVineBar : ModTile
	{
		public override void PostSetDefaults()
		{
			Main.tileShine[Type] = 800;
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileLighted[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.addTile(Type);

			ItemDrop = ModContent.ItemType<Items.CyanVineBar>();

			ModTranslation modTranslation = CreateMapEntryName(null);
			modTranslation.SetDefault("Metal Bar");
			AddMapEntry(new Color(224, 194, 101), modTranslation);
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			type = ModContent.DustType<Dusts.CyanVine>();
			return true;
		}		
		public override void NearbyEffects(int i, int j, bool closer)
		{
            if(Main.tile[i, j + 1].Slope != SlopeType.Solid || !Main.tile[i, j + 1].HasTile || Main.tile[i, j + 1].TileFrameY != 0 || Main.tile[i, j + 1].IsHalfBlock)
            {
                WorldGen.KillTile(i, j);
            }
		}
	}
}
