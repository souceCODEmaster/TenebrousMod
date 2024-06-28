using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using TenebrousMod.TenebrousModSystem;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;

namespace TenebrousMod.Items.Weapons.Ocean
{
    public class HydroArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 3));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.knockBack = 3;
            Item.width = 18;
            Item.height = 46;
            Item.maxStack = 9999;
            Item.FitsAmmoSlot();
            Item.ammo = AmmoID.Arrow;
            Item.rare = ItemRarityID.Lime;
            Item.consumable = true;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return WeaponLighting.LightingOnGround(Item, 1, spriteBatch, Color.LightSteelBlue, alphaColor, ref rotation, ref scale, whoAmI);
        }
        public override void PostUpdate()
        {
            WeaponLighting.PostLighting(Item, 1);
        }
    }
    public class HydroArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.FrostArrow);
            Projectile.width = 46;
            Projectile.height = 18;
            AIType = ProjectileID.FrostburnArrow;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 1800;
            Projectile.penetrate = 1;
            Projectile.damage = 14;
            Projectile.knockBack = 3;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 600);
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Water, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Water, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position); // Play the arrow collision sound
            Projectile.Kill(); // Destroy the projectile after colliding with tiles
            return false; // Return false to stop the projectile from moving
        }
        public override void AI()
        {
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }
    }
}