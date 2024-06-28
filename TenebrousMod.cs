using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TenebrousMod.Items.TreasureBags;
using System.IO;
using Terraria.ModLoader.IO;
using TenebrousMod.Items.Consumables;
using TenebrousMod.Items.Placeable;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace TenebrousMod
{
    public class TenebrousMod : Mod
    {
        internal Mod bossChecklist = null;
        internal static TenebrousMod Instance;
        public override void Load()
        {
            Instance = this;
            bossChecklist = null;
            ModLoader.TryGetMod("BossChecklist", out bossChecklist);
        }
        public override void Unload()
        {
            bossChecklist = null;
        }
    }
    public class TenebrousModPlayer : ModPlayer
    {
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            yield return new Item(ModContent.ItemType<StarterBag>(), 1);
        }
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            bool inWater = !attempt.inLava && !attempt.inHoney;

            int yourChanceDenominator = 6;

            if (inWater && Main.rand.NextBool(yourChanceDenominator))
            {
                itemDrop = ModContent.ItemType<WarCrate>();
                return;

            }

        }
    }
    public class ShimmerCrittersPlayer : ModPlayer
    {
        public bool shimmerBuff = false;
        public override void ResetEffects()
        {
            shimmerBuff = false;
        }
        public override void GetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
        {
            if (shimmerBuff)
                fishingLevel += 2;
        }
    }
    public class StatPlayer : ModPlayer
    {
        public int LifeFruits;
        public int ManaCrystals;

        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            health = StatModifier.Default;
            health.Base = LifeFruits * RavagedHeart.Health;
            mana = StatModifier.Default;
            mana.Base = ManaCrystals * RavagedHeart.Mana;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)LifeFruits);
            packet.Write((byte)ManaCrystals);
            packet.Send(toWho, fromWho);
        }
        public void ReceivePlayerSync(BinaryReader reader)
        {
            LifeFruits = reader.ReadByte();
            ManaCrystals = reader.ReadByte();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            StatPlayer clone = (StatPlayer)targetCopy;
            clone.LifeFruits = LifeFruits;
            clone.ManaCrystals = ManaCrystals;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            StatPlayer clone = (StatPlayer)clientPlayer;

            if (LifeFruits != clone.LifeFruits || ManaCrystals != clone.ManaCrystals)
                SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["RavagedHeartLife"] = LifeFruits;
            tag["RavagedHeartMana"] = ManaCrystals;
        }

        public override void LoadData(TagCompound tag)
        {
            LifeFruits = tag.GetInt("RavagedHeartLife");
            ManaCrystals = tag.GetInt("RavagedHeartMana");
        }
    }
}