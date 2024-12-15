namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class TownBrick_Gray_Wall_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<Walls.TownBrick_Gray_Wall>());
		Item.width = 24;
		Item.height = 24;
	}

	public override void AddRecipes()
	{
		CreateRecipe(4)
			.AddIngredient(ModContent.ItemType<TownBrick_Gray_Item>(), 1)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}