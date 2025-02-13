using Terraria.GameContent.Creative;
using Everglow.Commons.Utilities;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furniture.Furnace;

public class HeatproofSink_item : SinkItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnaceTiles.HeatproofSink>());
		base.SetDefaults();
	}
}