﻿using Terraria.Localization;

namespace MythMod.OceanMod.Tiles
{
    public class OceanBlueOre : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[(int)base.Type] = true;
            Main.tileMergeDirt[(int)base.Type] = false;
            Main.tileBlendAll[(int)base.Type] = false;
            Main.tileBlockLight[(int)base.Type] = true;
            Main.tileShine2[(int)base.Type] = false;
            Main.ugBackTransition = 1000;
            DustType = DustID.BorealWood;
            MinPick = 200;
            SoundType = SoundID.Grass;
            SoundStyle = 2;
            ItemDrop = ModContent.ItemType<OceanMod.Items.Ores.OceanBlueOre>();
            Main.tileSpelunker[(int)base.Type] = true;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(35, 126, 168), modTranslation);
            modTranslation.SetDefault("Ocean Blue Ore");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "沧流矿");
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
