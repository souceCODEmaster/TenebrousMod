using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace TenebrousMod.Items.Accessories
{
    public class ElementalBreaker : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 50;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.statLife < 0.25 * player.statLifeMax2)
            {
                player.AddBuff(BuffID.Swiftness, 2);
                Item.defense = 4;
            }
            else
                Item.defense = 0;
        }
    }
}
