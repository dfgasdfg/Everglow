using Terraria.DataStructures;
using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
    public class MeteorClub : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor Club");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "流星棍");
            Tooltip.SetDefault("Spin to hit enemies");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "旋转挥舞以攻击敌人");
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.damage = 16;
            Item.DamageType = DamageClass.Melee;
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 4;
            Item.rare = 1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 4;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.knockBack = 4f;
            //Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 0;
            Item.value = 500;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Projectiles.MeteorClub>();
            Item.shootSpeed = 0.04f;
        }
        private bool St = false;
        public override void HoldItem(Player player)
        {
            if (!Main.mouseLeft)
            {
                St = false;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!St && Main.mouseLeft)
            {
                St = true;
                if (player.ownedProjectileCounts[type] < 1)
                {
                    Projectile.NewProjectile(source, position + velocity * 2f, Vector2.Zero, type, damage, knockback, player.whoAmI, 0f, 0f);
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(117, 18)
                .AddTile(18)
                .Register();
        }
    }
}
