using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

internal class FeatheredStaff : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 50;
		Item.height = 54;

		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.useAnimation = 35;
		Item.useTime = 35;
		Item.UseSound = SoundID.Item20;
		Item.autoReuse = true;
		Item.noMelee = true;

		Item.damage = 12;
		Item.DamageType = DamageClass.Magic;
		Item.crit = 4;
		Item.knockBack = 3.25f;
		Item.mana = 6;

		Item.shoot = ModContent.ProjectileType<Projectiles.FeatheredStaff>();
		Item.shootSpeed = 12;

		Item.SetShopValues(
			ItemRarityColor.Green2,
			Item.buyPrice(silver: 20));
	}
}