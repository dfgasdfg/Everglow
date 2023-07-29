﻿using System;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Everglow.Ocean.Items.Walls
{
    public class OlivineBrickWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			// // base.DisplayName.SetDefault("");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "橄榄石晶莹宝石墙");
		}
		public override void SetDefaults()
		{
			base.Item.width = 24;
			base.Item.height = 24;
			base.Item.maxStack = 999;
			base.Item.useTurn = true;
			base.Item.autoReuse = true;
			base.Item.useAnimation = 15;
			base.Item.useTime = 7;
			base.Item.useStyle = 1;
			base.Item.consumable = true;
            base.Item.createWall = base.Mod.Find<ModWall>("橄榄石晶莹宝石墙").Type;
		}
		public override void AddRecipes()
		{
			Recipe modRecipe = /* base */Recipe.Create(this.Type, 4);
            modRecipe.AddIngredient(null, "OlivineBrick", 1);
			modRecipe.AddTile(18);
			modRecipe.Register();
            Recipe modRecipe2 = /* base */Recipe.Create(null, "OlivineBrick", 1);
            modRecipe2.AddIngredient(this, 4);
            modRecipe2.AddTile(18);
            modRecipe2.Register();
        }
        public override void PostUpdate()
        {
            Lighting.AddLight((int)((Item.position.X + Item.width / 2) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), 0.25f, 0.65f, 0f);
        }
    }
}
