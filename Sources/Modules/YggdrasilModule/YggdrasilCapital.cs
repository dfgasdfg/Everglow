﻿using Everglow.Sources.Modules.WorldModule;
namespace Everglow.Sources.Modules.YggdrasilModule
{
    public class YggdrasilCapital : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = false;
        }
        public override bool? UseItem(Player player)
        {
            if (player.itemAnimation == player.itemAnimationMax)
            {
                //TestCode
                if (WorldManager.Activing<YggdrasilWorld>())
                {
                    WorldManager.TryBack();
                }
                else
                {
                    WorldManager.TryEnter<YggdrasilWorld>();
                }
            }
            return base.UseItem(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .Register();
        }
    }
}
