namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class StarDancer : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 12;
		Item.value = 1114;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.StarDancer>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.StarDancer_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.FallenStar, 20)
			.AddIngredient(ItemID.SunplateBlock, 50)
			.AddTile(TileID.SkyMill)
			.Register();
	}
}
