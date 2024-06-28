using Terraria.ModLoader;
using Terraria;

namespace TenebrousMod.Buffs
{
    public class InternalImploding : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) /= 1.20f;
            player.statDefense /= 1.20f;
        }
    }
}
