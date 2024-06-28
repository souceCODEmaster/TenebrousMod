using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using TenebrousMod.Buffs;
using TenebrousMod.Items.Bars;
using Microsoft.Xna.Framework.Graphics;
using TenebrousMod.TenebrousModSystem;
using Microsoft.Xna.Framework;

namespace TenebrousMod.Items.Armor.TheBehemothsSet
{
    [AutoloadEquip(EquipType.Body)]
    public class TheBehemothsBreastplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 6);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {

        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<TheBehemothsBar>(20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        [AutoloadEquip(EquipType.Head)]
        public class TheBehemothsHeadgear : ModItem
        {
            public override void SetStaticDefaults()
            {
                ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
            }
            public override void UpdateEquip(Player player)
            {
                player.GetCritChance(DamageClass.Ranged) += 0.10f;
                player.GetDamage(DamageClass.Ranged) *= 1.10f;
            }
            public override void SetDefaults()
            {
                Item.width = 20;
                Item.height = 22;
                Item.value = Item.sellPrice(gold: 3);
                Item.rare = ItemRarityID.LightRed;
                Item.defense = 11;
            }

            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return body.type == ModContent.ItemType<TheBehemothsBreastplate>() && legs.type == ModContent.ItemType<TheBehemothsGreaves>();
            }
            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "UNKNOWN PLACEHOLDER";
            }

            public override void AddRecipes()
            {
                CreateRecipe().AddIngredient<TheBehemothsBar>(12)
                    .AddTile(TileID.MythrilAnvil)
                    .Register();
            }
        }
        [AutoloadEquip(EquipType.Head)]
        public class TheBehemothsHelmet : ModItem
        {
            public override void UpdateEquip(Player player)
            {
                player.GetCritChance(DamageClass.Melee) += 0.10f;
                player.GetDamage(DamageClass.Melee) *= 1.10f;
            }
            public override void SetStaticDefaults()
            {
                ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
            }

            public override void SetDefaults()
            {
                Item.width = 16;
                Item.height = 24;
                Item.value = Item.sellPrice(gold: 3);
                Item.rare = ItemRarityID.LightRed;
                Item.defense = 13;
            }

            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return body.type == ModContent.ItemType<TheBehemothsBreastplate>() && legs.type == ModContent.ItemType<TheBehemothsGreaves>();
            }

            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "Melee Speed increased by 10%";
                player.pickSpeed += 0.10f;

            }
            public override void AddRecipes()
            {
                CreateRecipe().AddIngredient<TheBehemothsBar>(15)
                    .AddTile(TileID.MythrilAnvil)
                    .Register();
            }
        }
        [AutoloadEquip(EquipType.Legs)]
        public class TheBehemothsGreaves : ModItem
        {

            public override void SetDefaults()
            {
                Item.width = 22;
                Item.height = 18;
                Item.value = Item.sellPrice(gold: 3, silver: 25);
                Item.rare = ItemRarityID.LightRed;
                Item.defense = 9;
            }

            public override void UpdateEquip(Player player)
            {
                player.moveSpeed += 1.10f;
                player.AddBuff(BuffID.WaterWalking, 1);
            }
            public override void AddRecipes()
            {
                CreateRecipe().AddIngredient<TheBehemothsBar>(15)
                    .AddTile(TileID.MythrilAnvil)
                     .Register();
            }
        }
    }
}