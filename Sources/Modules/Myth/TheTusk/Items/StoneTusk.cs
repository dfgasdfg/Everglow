namespace Everglow.Myth.TheTusk.Items;
//TODO:Translate:风化獠牙
public class StoneTusk : ModItem
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Fossilized Tusk");
	}
	public override void SetDefaults()
	{
		Item.width = 32;
		Item.height = 24;
		Item.maxStack = 999;
		Item.value = 100;
		Item.rare = ItemRarityID.Blue;
	}
}
