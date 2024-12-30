namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Items.Furnitures;

public class TwilightEucalyptusDoor : DoorItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles.Furnitures.TwilightEucalyptusDoorClosed>());
		base.SetDefaults();
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		// Add recipe.
		recipe.Register();
	}
}