using TenebrousMod.Items.Food;
using TenebrousMod.Items.Placeable;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebrousMod.Items.TreasureBags
{
    public class StarterBag : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 36;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MainMenuThemeDayMusicBox>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MainMenuThemeNightMusicBox>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ItemID.IronBroadsword, 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ItemID.LesserHealingPotion, 1, 3, 3));
            itemLoot.Add(ItemDropRule.Common(ItemID.LesserManaPotion, 1, 3, 3));
            itemLoot.Add(ItemDropRule.Common(ItemID.IronskinPotion, 1, 3, 3));
            itemLoot.Add(ItemDropRule.Common(ItemID.Apple, 1, 10, 10));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Meat>(), 1, 5, 5));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BatteryJuice>(), 1, 5, 5));
            itemLoot.Add(ItemDropRule.Common(ItemID.WoodenCrate, 1, 3, 3));
            itemLoot.Add(ItemDropRule.Common(ItemID.Chest, 1, 3, 3));
        }
    }
}