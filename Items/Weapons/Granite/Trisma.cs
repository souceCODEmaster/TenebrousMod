using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebrousMod.Items.Weapons.Granite
{
    public class Trisma : ModItem
    {
        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 70;
            Item.useAmmo = AmmoID.Arrow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 32;
            Item.height = 56;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.LightPurple;
            Item.shootSpeed = 8f;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.UseSound = SoundID.Item5;
            Item.shoot = ProjectileID.HellfireArrow;
        }
    }
}
