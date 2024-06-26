﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebrousMod.Items.Accessories
{
    public class EtherealEmblem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 26;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 2;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<GenericDamageClass>() *= 1.05f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LifeCrystal, 1)
                .AddIngredient(ItemID.ManaCrystal, 1)
                .AddIngredient(ItemID.BandofRegeneration, 1)
                .AddIngredient(ItemID.BandofStarpower, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
