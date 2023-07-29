﻿using Terraria.Localization;

namespace Everglow.Ocean.Items
{
    public class ReefSharkSpecimen : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Reefshark Specimen");
            // DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "礁鲨标本");
        }
        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 42;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.maxStack = 999;
            Item.rare = 1;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<Tiles.ReefSharkSpecimen>();
        }
    }
}
