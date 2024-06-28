using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using TenebrousMod.Items.Materials;
using TenebrousMod.TenebrousModSystem;

namespace TenebrousMod.Items.Weapons.Granite
{
    public class GraniteBlaster : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 22;
            Item.useTime = 30;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundPlayerTM.Sound("TenebrousMod/Assets/SFX/GunPowerful");
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GraniteOrbProj>();
            Item.shootSpeed = 16f;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Granite, 25)
                .AddIngredient<GraniteShard>(1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class GraniteOrbProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.99f;
            Projectile.scale *= 1.002f;

            if (Projectile.velocity.Length() < 1f)
            {
                Projectile.velocity = Vector2.Zero;
                Explode();
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return base.OnTileCollide(oldVelocity);
        }
        private void Explode()
        {
            int explosionRadius = 100;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (target.active && !target.friendly && Vector2.Distance(Projectile.Center, target.Center) <= explosionRadius)
                {

                }
            }

            for (int i = 0; i < 30; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Granite);
            }

            Projectile.active = false;
        }
    }
}
