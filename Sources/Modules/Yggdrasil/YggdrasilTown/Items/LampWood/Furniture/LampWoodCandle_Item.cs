using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood.Furniture;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.LampWood.Furniture;

public class LampWoodCandle_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<LampWoodCandle>());
	}
}