﻿using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using TenebrousMod.NPCs.Bosses.Emberwing;

namespace TenebrousMod.NPCs.Bosses.TheGreatHarpy
{
    public class TheGreatHarpy : ModNPC
    {
        private bool auraActive = true;
        private int auraRadius = 0;
        private int dashCooldown = 0;
        private int minionSpawnCooldown = 0;
        private int attackTimer = 0;
        public override void SetDefaults()
        {
            NPC.width = 78;
            NPC.height = 52;
            NPC.lifeMax = 1500;
            NPC.damage = 30;
            NPC.defense = 5;
            NPC.knockBackResist = 0f;
            NPC.scale = 2;
            NPC.value = Item.buyPrice(gold: 2, silver: 50);
            NPC.aiStyle = -1;
            dashCooldown = 0;
            minionSpawnCooldown = 0;
        }
        public override void OnSpawn(IEntitySource source) => SpawnHarpyMinion();

        public override void SetStaticDefaults()
        {
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        private bool CheckTowardsPlayerX(Player player, Vector2 offset)
        {
            float target = player.Center.X + offset.X;
            if (Math.Abs(NPC.Center.X + NPC.velocity.X - target) <= Math.Abs(NPC.Center.X - target))
                return true;
            return false;
        }

        private bool CheckTowardsPlayerY(Player player, Vector2 offset)
        {
            float target = player.Center.Y + offset.Y;
            if (Math.Abs(NPC.Center.Y + NPC.velocity.Y - target) <= Math.Abs(NPC.Center.Y - target))
                return true;
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            Move();
            NPC.direction = NPC.spriteDirection = NPC.Center.X < player.Center.X ? 1 : -1; 
            Vector2 offset = Vector2.Zero;
            if (player.dead)
            {
                NPC.velocity.Y -= 0.4f;
                NPC.EncourageDespawn(10);
                return;
            }
            if (auraActive)
            {
                if (auraRadius < 5)
                    auraRadius++;
                else
                    auraActive = false;
            }

            if (!auraActive)
            {
                if (auraRadius > 0)
                    auraRadius--;
            }

            if (auraActive && auraRadius == 5)
            {
                dashCooldown++;
                minionSpawnCooldown++;

                if (dashCooldown >= 900) // Every 15 seconds
                {
                    for (int i = 0; i < 3; i++)
                    {
                        DoDashAttack();
                    }
                    dashCooldown = 0;
                }
                NPC.rotation = (float)Math.Sqrt(NPC.velocity.X) / 100;
                offset = Vector2.Zero;
                NPC.velocity.X += 0.1f * NPC.direction;
                if (!CheckTowardsPlayerX(player, offset))
                {
                    NPC.velocity.X *= 0.99f;
                }
                if (NPC.Center.Y > player.Center.Y)
                {
                    NPC.velocity.Y += -0.1f;
                }
                else
                {
                    NPC.velocity.Y += 0.1f;
                }
                if (!CheckTowardsPlayerY(player, offset))
                {
                    NPC.velocity.Y *= 0.875f;
                }
                if (minionSpawnCooldown >= 1200) // Every 20 seconds
                {
                    SpawnHarpyMinion();
                    minionSpawnCooldown = 0;
                }
            }
        }
        private Vector2 Alignment(Vector2 current, Vector2 target, float distance, float pull)
        {
            Vector2 NewVelocity = Vector2.Zero;
            Vector2 vector1 = Vector2.Zero;
            vector1 = current - target;
            float magnitude = Vector2.Zero.Distance(vector1);
            vector1 = Vector2.Normalize(vector1);
            distance -= magnitude;
            pull *= distance;
            NewVelocity += vector1 * pull;
            return NewVelocity;
        }
        private void Move()
        {
            Player player = Main.player[NPC.target];
            attackTimer++;
            if (attackTimer % 1200 == 0)
            {
                SpawnHarpyMinion();
            }
            NPC.rotation = (float)Math.Sqrt(NPC.velocity.X) / 100;
            Vector2 offset = Vector2.Zero;
            offset = new Vector2(0, -240).RotatedBy(MathHelper.ToRadians(attackTimer));
            Vector2 movement = Alignment(NPC.Center, (Main.player[NPC.target].Center + offset), 0, 0.05f)/*(NPC.position - (Main.player[NPC.target].position + offset))*/;
            NPC.velocity = movement;
        }
        private void DoDashAttack()
        {
            Player player = Main.player[NPC.target];
            Vector2 Ppos = player.position;
            Vector2 Npos = NPC.position;
            NPC.velocity = new Vector2(Ppos.X - Npos.X, Ppos.Y - Npos.Y);
        }

        private void SpawnHarpyMinion()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int harpyType = NPCID.Harpy; // Replace with your custom harpy minion type if needed
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, harpyType);
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(auraActive);
            writer.Write(auraRadius);
            writer.Write(dashCooldown);
            writer.Write(minionSpawnCooldown);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            auraActive = reader.ReadBoolean();
            auraRadius = reader.ReadInt32();
            dashCooldown = reader.ReadInt32();
            minionSpawnCooldown = reader.ReadInt32();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
    }
}