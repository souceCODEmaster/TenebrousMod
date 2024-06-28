using Microsoft.Xna.Framework;
using System.Transactions;
using TenebrousMod.NPCs.Bosses.Emberwing;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebrousMod.Items.Weapons.Crimson
{
    public class Bloodlust : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            int projectile = ProjectileID.InfernoFriendlyBolt;
            Item.damage = 3;
            Item.mana = 10;
            Item.DamageType = DamageClass.Magic;
            Item.width = 38;
            Item.height = 38;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(gold: 2, silver: 50);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item71;

            Item.autoReuse = true;
            Item.shoot = projectile;
            Item.shootSpeed = 3f;
            // TODO: recheck what was this logic supposed to do
            // 'projectile' is a type here, not a valid index
            // Main.projectile[projectile].scale = 0.5f;
            // Main.projectile[projectile].tileCollide = true;
        }
    }
    //public class FriendlyBloodShot : ModProjectile
    // {
    //    public override string Texture => ProjectileID.BloodShot;
    //    public override void SetDefaults()
    //     {
    //        Projectile.width = 16;
    //        Projectile.height = 16;
    //        Projectile.friendly = true;
    //        Projectile.aiStyle = 1;
    //        Projectile.timeLeft = 600;
    //        Projectile.penetrate = 2;
    //        Projectile.tileCollide = true;
    //        Projectile.ignoreWater = true;
    //    }
    // }
}
