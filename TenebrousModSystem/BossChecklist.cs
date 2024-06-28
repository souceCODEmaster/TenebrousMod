using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenebrousMod.Items.Summons;
using TenebrousMod.NPCs.Bosses.Emberwing;
using TenebrousMod.NPCs.Bosses.Icerus;
using TenebrousMod.NPCs.Bosses.TheBehemoth;
using TenebrousMod.NPCs.Bosses.TheGreatHarpy;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TenebrousMod.TenebrousModSystem
{
    internal class BossChecklistIntegration : ModSystem
    {
        Mod bossChecklistMod;
        public override void Load()
        {
        }
        public override void PostSetupContent()
        {
            try
            {
                if (!ModLoader.TryGetMod("BossChecklist", out bossChecklistMod))
                    return;

                RegisterBoss<TheGreatHarpy>(1.6f, () => false, new List<int> { 0 }, () => Mod.GetLocalization("Mods.TenebrousMod.BossChecklistIntegration.TheGreatHarpy.TheGreatHarpySpawnInfo"));
                RegisterBoss<DesertBehemoth>(7.1f, () => false, new List<int> { 0 }, () => Mod.GetLocalization("Mods.TenebrousMod.BossChecklistIntegration.TheBehemoth.TheBehemothSpawnInfo"));
                RegisterBoss<IcerusBossHead>(7.2f, () => false, new List<int> { ModContent.ItemType<FrozenMoral>() }, () => Mod.GetLocalization("Mods.TenebrousMod.BossChecklistIntegration.Icerus.IcerusSpawnInfo"));
                RegisterBoss<Emberwing>(12.1f, () => false, new List<int> { ModContent.ItemType<EmberPact>() }, () => Mod.GetLocalization("Mods.TenebrousMod.BossChecklistIntegration.Emberwing.EmberwingSpawnInfo"));

            }
            finally
            {
                // the instance is not really required for anything else 
                // so it could just be set to null after all bosses have been registered
                // this also prevents this mod from keeping boss checklist alive in case this mod doesn't unload
                bossChecklistMod = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <inheritdoc cref="RegisterBoss"/>
        void RegisterBoss<T>(float progression, Func<bool> downed, List<int> spawnItems = null, Func<LocalizedText> spawnInfo = null, List<int> collectibles = null, Func<bool> isAvailable = null, string internalName = null) where T : ModNPC
        {
            RegisterBoss(ModContent.NPCType<T>(), progression, downed, spawnItems, spawnInfo, isAvailable, collectibles, ModContent.GetInstance<T>().Name);
        }
        /*
         * Boss priorities according to boss checklist's wiki https://github.com/JavidPack/BossChecklist/wiki/Boss-Progression-Values
         * KingSlime = 1f;
         * TorchGod = 1.5f;
         * EyeOfCthulhu = 2f;
         * BloodMoon = 2.5f;
         * EaterOfWorlds = 3f;
         * GoblinArmy = 3.33f;
         * OldOnesArmy = 3.66f;
         * DarkMage = 3.67f;
         * QueenBee = 4f;
         * Skeletron = 5f;
         * DeerClops = 6f;
         * WallOfFlesh = 7f;
         * FrostLegion = 7.33f;
         * PirateInvasion = 7.66f;
         * PirateShip = 7.67f;
         * QueenSlime = 8f;
         * TheTwins = 9f;
         * TheDestroyer = 10f;
         * SkeletronPrime = 11f;
         * Ogre = 11.01f;
         * SolarEclipse = 11.5f;
         * Plantera = 12f;
         * Golem = 13f;
         * PumpkinMoon = 13.25f;
         * MourningWood = 13.26f;
         * Pumpking = 13.27f;
         * FrostMoon = 13.5f;
         * Everscream = 13.51f;
         * SantaNK1 = 13.52f;
         * IceQueen = 13.53f;
         * MartianMadness = 13.75f;
         * MartianSaucer = 13.76f;
         * DukeFishron = 14f;
         * EmpressOfLight = 15f;
         * Betsy = 16f;
         * LunaticCultist = 17f;
         * LunarEvent = 17.01f;
         * Moonlord = 18f;
         */

        // https://github.com/JavidPack/BossChecklist/wiki/%5B1.4.4%5D-Boss-Log-Entry-Mod-Call
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"> The type of the boss.</param>
        /// <param name="progression"> <see href="https://github.com/JavidPack/BossChecklist/wiki/Boss-Progression-Values"/>  </param>
        /// <param name="downed"> A function that returns if the boss has been killed, usually just <c>()=>MySystem.DownedMyBoss</c>  </param>
        /// <param name="spawnItems"> List of item types that spawn the boss </param>
        /// <param name="spawnInfo"> A function that returns a LocalizedText for the boss' spawn info </param>
        /// <param name="collectibles"> Masks, pets, relics and trophies, music boxes, etc. <seealso href="https://github.com/JavidPack/BossChecklist/wiki/%5B1.4.4%5D-Boss-Log-Entry-Mod-Call#collectibles"/>  </param>
        /// <param name="isAvailable"> A function that indicates if a boss is visible on boss checklist's lists, if the function returns <see langword="false"/>, the boss is hidden.  </param>
        /// <param name="internalName">The internal name for the boss checklist entry. <br />Will use the ModNPC's Name (class name) if <see langword="null"/></param>
        void RegisterBoss(int type,
            float progression,
            Func<bool> downed,
            List<int> spawnItems = null,
            Func<LocalizedText> spawnInfo = null,
            Func<bool> isAvailable = null,
            List<int> collectibles = null,
            string internalName = null)
        {
            if (bossChecklistMod == null)
            {
                return;
            }

            if (internalName == null)
            {
                if (NPCLoader.GetNPC(type)?.Name is not { } name)
                {
                    throw new InvalidOperationException("Possible attempt to register a vanilla npc on boss checklist? the Name or the ModNPC are null");
                }
                internalName = name;
            }
            Dictionary<string, object> extra = new();
            if (spawnItems != null)
                extra["spawnItems"] = extra;
            if (collectibles != null)
                extra["collectibles"] = collectibles;
            if (isAvailable != null)
                extra["availability"] = isAvailable;

            bossChecklistMod.Call("LogBoss", Mod, internalName, progression, downed, new List<int>() { type }, extra);
        }

    }
}