using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using TenebrousMod.Buffs;
using TenebrousMod.Items.Bars;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using TenebrousMod.TenebrousModSystem;
using Microsoft.Xna.Framework;

namespace TenebrousMod.Items.Armor.ObscurumSet
{
    [AutoloadEquip(EquipType.Body)]
    public class ObscurumPlate : ModItem
    {

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return WeaponLighting.LightingOnGround(Item, 1, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
        public override void PostUpdate()
        {
            WeaponLighting.PostLighting(Item, 1);
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 22;
        }

        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.OnFire] = true;
            player.GetCritChance(DamageClass.Generic) += 1.08f;
            player.AddBuff(ModContent.BuffType<Infuriated>(), 0);
            if (player.statLife < 0.25 * player.statLifeMax2)
                player.AddBuff(BuffID.Inferno, 1);
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<ObscurumBar>(25)
                .AddTile(TileID.AdamantiteForge)
                .Register();
        }
        [AutoloadEquip(EquipType.Head)]
        public class ObscurumHelmetMelee : ModItem
        {
            public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
            {
                return WeaponLighting.LightingOnGround(Item, 1, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
            }
            public override void PostUpdate()
            {
                WeaponLighting.PostLighting(Item, 1);
            }
            public override void SetStaticDefaults()
            {
                ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

            }

            public override void SetDefaults()
            {
                Item.width = 32;
                Item.height = 24;
                Item.value = Item.sellPrice(gold: 6);
                Item.rare = ItemRarityID.LightPurple;
                Item.defense = 26;
            }
            public override void UpdateEquip(Player player)
            {
                player.GetDamage(DamageClass.Melee) *= 1.05f;
            }
            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return body.type == ModContent.ItemType<ObscurumPlate>() && legs.type == ModContent.ItemType<ObscurumGreaves>();
            }
            public override void UpdateArmorSet(Player player)
            {

                player.setBonus = "Melee Damage increased by 15%";
                player.GetDamage(DamageClass.Melee) *= 1.15f;
            }

            public override void AddRecipes()
            {
                CreateRecipe().AddIngredient<ObscurumBar>(25)
                    .AddTile(TileID.AdamantiteForge)
                    .Register();
            }
        }
        [AutoloadEquip(EquipType.Head)]
        public class ObscurumHeadgearRanger : ModItem
        {

            public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
            {
                return WeaponLighting.LightingOnGround(Item, 1, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
            }
            public override void PostUpdate()
            {
                WeaponLighting.PostLighting(Item, 1);
            }
            public override void SetStaticDefaults()
            {
                ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
            }
            public override void UpdateEquip(Player player)
            {
                player.GetDamage(DamageClass.Ranged) *= 1.05f;
            }
            public override void SetDefaults()
            {
                Item.width = 32;
                Item.height = 30;
                Item.value = Item.sellPrice(gold: 5);
                Item.rare = ItemRarityID.LightPurple;
                Item.defense = 15;
            }

            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return body.type == ModContent.ItemType<ObscurumPlate>() && legs.type == ModContent.ItemType<ObscurumGreaves>();
            }
            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "Ranger Damage increased by 15%";
                player.GetDamage(DamageClass.Ranged) *= 1.15f;
            }

            public override void AddRecipes()
            {
                CreateRecipe().AddIngredient<ObscurumBar>(20)
                    .AddTile(TileID.AdamantiteForge)
                    .Register();
            }
        }
        [AutoloadEquip(EquipType.Head)]
        public class ObscurumHoodMage : ModItem
        {
            public override void UpdateEquip(Player player)
            {
                player.GetDamage(DamageClass.Magic) *= 1.05f;
            }
            public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
            {
                return WeaponLighting.LightingOnGround(Item, 1, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
            }
            public override void PostUpdate()
            {
                WeaponLighting.PostLighting(Item, 1);
            }
            public override void SetStaticDefaults()
            {
                ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
            }

            public override void SetDefaults()
            {
                Item.width = 32;
                Item.height = 26;
                Item.value = Item.sellPrice(gold: 4);
                Item.rare = ItemRarityID.LightPurple;
                Item.defense = 12;
            }

            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return body.type == ModContent.ItemType<ObscurumPlate>() && legs.type == ModContent.ItemType<ObscurumGreaves>();
            }
            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "Magic Damage increased by 15%";
                player.GetDamage(DamageClass.Magic) *= 1.15f;
            }

            public override void AddRecipes()
            {
                CreateRecipe().AddIngredient<ObscurumBar>(15)
                    .AddTile(TileID.AdamantiteForge)
                    .Register();
            }
        }

        [AutoloadEquip(EquipType.Head)]
        public class ObscurumMaskSummoner : ModItem
        {
            public override void UpdateEquip(Player player)
            {
                player.GetDamage(DamageClass.Summon) *= 1.05f;
            }
            public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
            {
                return WeaponLighting.LightingOnGround(Item, 1, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
            }
            public override void PostUpdate()
            {
                WeaponLighting.PostLighting(Item, 1);
            }
            public override void SetStaticDefaults()
            {
                ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
            }

            public override void SetDefaults()
            {
                Item.width = 26;
                Item.height = 28;
                Item.value = Item.sellPrice(gold: 3);
                Item.rare = ItemRarityID.LightPurple;
                Item.defense = 8;
            }

            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return body.type == ModContent.ItemType<ObscurumPlate>() && legs.type == ModContent.ItemType<ObscurumGreaves>();
            }

            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "Summoner Damage increased by 15%";
                player.GetDamage(DamageClass.Summon) *= 1.15f;

            }
            public override void AddRecipes()
            {
                CreateRecipe().AddIngredient<ObscurumBar>(12)
                    .AddTile(TileID.AdamantiteForge)
                    .Register();
            }
        }
        [AutoloadEquip(EquipType.Legs)]
        public class ObscurumGreaves : ModItem
        {
            public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
            {
                return WeaponLighting.LightingOnGround(Item, 1, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
            }
            public override void PostUpdate()
            {
                WeaponLighting.PostLighting(Item, 1);
            }

            public override void SetDefaults()
            {
                Item.width = 22;
                Item.height = 18;
                Item.value = Item.sellPrice(gold: 3, silver: 25);
                Item.rare = ItemRarityID.LightPurple;
                Item.defense = 14;
            }

            public override void UpdateEquip(Player player)
            {
                player.moveSpeed += 1.10f;
                player.AddBuff(BuffID.WaterWalking, 1);
            }
            public override void AddRecipes()
            {
                CreateRecipe().AddIngredient<ObscurumBar>(15)
                     .AddTile(TileID.AdamantiteForge)
                     .Register();
            }
        }
        [AutoloadEquip(EquipType.Wings)]
        public class ObscurumWings : ModItem
        {
            public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
            {
                return WeaponLighting.LightingOnGround(Item, 1, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
            }
            public override void PostUpdate()
            {
                WeaponLighting.PostLighting(Item, 1);
            }
            public override void SetStaticDefaults()
            {
                ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 9f, 2.5f);
            }

            public override void SetDefaults()
            {
                Item.width = 30;
                Item.height = 28;
                Item.value = Item.sellPrice(gold: 4, silver: 50);
                Item.rare = ItemRarityID.LightPurple;
                Item.accessory = true;
            }

            public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
                ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
            {
                ascentWhenFalling = 0.85f;
                ascentWhenRising = 0.15f;
                maxCanAscendMultiplier = 1f;
                maxAscentMultiplier = 3f;
                constantAscend = 0.135f;
            }

            public override void AddRecipes()
            {
                CreateRecipe().AddIngredient<ObscurumBar>(20)
                    .AddIngredient(ItemID.SoulofFlight, 20)
                     .AddTile(TileID.AdamantiteForge)
                     .Register();
            }
        }
    }
}
