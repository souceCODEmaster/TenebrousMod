using Terraria.ModLoader;
using Terraria;

namespace TenebrousMod.Buffs
{
    public class Infuriated : ModBuff
    {
        public static readonly float DamageBonus = 1.10f;
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense /= 1.10f;
            player.GetDamage(DamageClass.Generic) *= DamageBonus;
        }
    }
}
