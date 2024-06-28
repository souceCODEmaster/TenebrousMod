using Terraria.ID;
using Terraria.ModLoader.Utilities;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using TenebrousMod.Items.Accessories;
using TenebrousMod.Items.Materials;
namespace TenebrousMod.NPCs.Enemies.Space
{
    public class AngelEye : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2; // make sure to set this for your modnpcs.

            // Specify the debuffs it is immune to.
            // This NPC will be immune to the Poisoned debuff.
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 32; // The width of the npc's hitbox (in pixels)
            NPC.width = 36; // The width of the npc's hitbox (in pixels)
            NPC.height = 22; // The height of the npc's hitbox (in pixels)
            NPC.aiStyle = NPCAIStyleID.DemonEye; // This npc has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
            NPC.damage = 13; // The amount of damage that this npc deals
            NPC.defense = 4; // The amount of defense that this npc has
            NPC.lifeMax = 36; // The amount of health that this npc has
            NPC.HitSound = SoundID.NPCHit1; // The sound the NPC will make when being hit.
            NPC.DeathSound = SoundID.NPCDeath1; // The sound the NPC will make when it dies.
            NPC.value = Item.buyPrice(silver: 28); // How many copper coins the NPC will drop when killed.
            AnimationType = NPCID.DemonEye;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            // we would like this npc to spawn in the overworld.
            return SpawnCondition.Sky.Chance * 1.5f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Lens, 1));
            npcLoot.Add(ItemDropRule.Common(ItemID.BlackLens, 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StarEssence>(), 2, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ElementalBreaker>(), 50, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ItemID.Star, 1));
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				// Sets your NPC's flavor text in the bestiary.
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement()
            });


        }
    }
}
