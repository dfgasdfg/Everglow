using Everglow.Myth.TheFirefly.Items;
using Terraria.GameContent.Creative;

namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class GlowWoodChandelierType3 : ModItem
{
	//TODO:Translate:流萤吊灯\n款式3
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.GlowWoodChandelierType3>());
		Item.width = 26;
		Item.height = 30;
		Item.value = 2000;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 4);
		recipe.AddIngredient(ModContent.ItemType<GlowWoodTorch>(), 4);
		recipe.AddIngredient(ItemID.Chain, 1);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}