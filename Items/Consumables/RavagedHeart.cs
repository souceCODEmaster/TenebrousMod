using TenebrousMod.Items.Bars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebrousMod.Items.Consumables
{
    internal class RavagedHeart : ModItem
    {
        public static readonly int Mana = 50;
        public static readonly int Health = 100;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LifeFruit);
            Item.consumable = true;
            Item.rare = ItemRarityID.Lime;
            Item.width = 18;
            Item.height = 26;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ConsumedLifeCrystals == Player.LifeCrystalMax && player.ConsumedLifeFruit == Player.LifeFruitMax;
        }


        public override bool? UseItem(Player player)
        {
            if (player.GetModPlayer<StatPlayer>().LifeFruits >= 1)
            {
                return null;
            }
            player.UseHealthMaxIncreasingItem(Health);
            player.GetModPlayer<StatPlayer>().LifeFruits++;

            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<ObscurumBar>(), 10)
            .AddIngredient(ItemID.LifeCrystal, 3)
            .AddIngredient(ItemID.CrystalShard, 15)
            .AddTile(TileID.AdamantiteForge)
            .Register();
        }
    }
}