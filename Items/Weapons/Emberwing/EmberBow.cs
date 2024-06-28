using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TenebrousMod.Items.Bars;
using TenebrousMod.TenebrousModSystem;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebrousMod.Items.Weapons.Emberwing
{
    public class EmberBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 76;
            Item.useAmmo = AmmoID.Arrow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 26;
            Item.height = 54;
            Item.value = Item.sellPrice(gold: 4, silver: 25);
            Item.rare = ItemRarityID.LightPurple;
            Item.shootSpeed = 6f;
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.UseSound = SoundID.Item5;
            Item.shoot = ModContent.ProjectileType<EmberArrowProj>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ObscurumBar>(10)
                .AddTile(TileID.AdamantiteForge)
                .Register();
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return WeaponLighting.LightingOnGround(Item, 1, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }

        public override void PostUpdate()
        {
            WeaponLighting.PostLighting(Item, 1);
        }
    }
}
