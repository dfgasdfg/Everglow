namespace Everglow.Myth.TheFirefly.Items;

public class LiveFluorescentWoodWall : ModItem
{
	//TODO:Translate:流萤生命木墙
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<Walls.LiveFluorescentWoodWall>());
		Item.width = 24;
		Item.height = 24;
	}
}