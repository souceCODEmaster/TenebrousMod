using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TenebrousMod.Global.NPCs.Bosses
{
    public class BossKill : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.WallofFlesh)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    string text = Language.GetTextValue("I'LL GET YOU FOR THIS!!!", Lang.GetNPCNameValue(npc.type));
                    Main.NewText(text, 150, 250, 150);
                }
            }
            base.OnKill(npc);
        }
    }
}
