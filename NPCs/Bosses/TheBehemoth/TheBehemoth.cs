using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ReLogic.Content;
using Terraria.Audio;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Terraria.DataStructures;
using TenebrousMod.Items.Weapons.Icerus;
using Terraria.GameContent.ItemDropRules;
using TenebrousMod.Items.Weapons.TheBehemoth;
using Terraria.ModLoader.Utilities;

namespace TenebrousMod.NPCs.Bosses.TheBehemoth
{
    [AutoloadBossHead]
    public class DesertBehemoth : ModNPC
    {
        private float CanSpawn;
        public override string BossHeadTexture => "TenebrousMod/NPCs/Bosses/TheBehemoth/TheBehemoth_Head_Boss";
        public override string Texture => "Terraria/Images/NPC_" + NPCID.SandShark;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 7;
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.SandShark];
        }
        public override void SetDefaults()
        {
            NPC.width = 25;
            NPC.height = 25;
            NPC.damage = 55;
            NPC.defense = 18;
            NPC.lifeMax = 9000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = Item.buyPrice(gold: 5);
            AnimationType = NPCID.SandShark;
            NPC.boss = true;
            NPC.aiStyle = -1;
            NPC.noGravity = false;
            NPC.knockBackResist = 0f;
            NPC.scale = 2f;
            attacking = false;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/TheBehemothTheme");
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<DesertBehemoth>()) && Main.hardMode)
            {
                return SpawnCondition.OverworldDayDesert.Chance * 2f;
            }
            return SpawnCondition.OverworldDayDesert.Chance * 0f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //if (Main.expertMode) npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<EmberwingTreasureBag>()));
            if (!Main.expertMode)
            {
                //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EmberwingTrophyI>(), 10));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BlunderBuss>(), 2));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WizenedGlave>(), 2));
            }
            //npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<EmberwingRelicI>()));

            //npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<MinionBossPetItem>(), 4));
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            //notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MinionBossMask>(), 7));

            npcLoot.Add(notExpertRule);
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }
        public override bool? CanFallThroughPlatforms()
        {
            return fallThroughPlatforms;
        }
        private void LookAtPlayer(Player player)
        {

            if (player.position.X > NPC.position.X)
                NPC.direction = 1;
            else
                NPC.direction = -1;

        }

        private void spawnWeakSandsharks()
        {

            var shark = NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCID.SandShark);
            shark.lifeMax = 50;
            shark.life = 50;
            shark.damage = 25;
            shark.defense = 3;

        }

        private enum attacks { Attack1 = 0, Attack2, Attack3, toFlightStage, Idle, MidAir }
        private enum stages { land, sky }

        private attacks currentState = attacks.Idle;
        public ref float AttackCounter => ref NPC.ai[0];
        public ref float AttackTimer => ref NPC.ai[1];
        public ref float AttackCooldown => ref NPC.ai[2];
        public ref float IdleDuration => ref NPC.ai[3];

        bool attacking = false;

        bool isFlyingStage = false;

        bool fallThroughPlatforms = false;

        bool isBurrowing = false;

        float alphaValue = 1f;

        int nextAttack = 1;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            if (attacking && currentState == attacks.Attack1 && !isFlyingStage)
            {
                Asset<Texture2D> texture = TextureAssets.Npc[Type];
                Vector2 origin = new Vector2(texture.Width() * 0.5f, texture.Height() / 4 * 0.5f);
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Vector2 pos = NPC.oldPos[k] - new Vector2(0, 64) - Main.screenPosition + origin + new Vector2(0f, NPC.gfxOffY);
                    Color color = NPC.GetAlpha(Color.White) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                    Main.EntitySpriteDraw(texture.Value, pos, new Rectangle(0, 0, texture.Width(), texture.Height() / 4), color, NPC.rotation, origin, NPC.scale, NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }

            }



            return true;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return isFlyingStage ? Color.CornflowerBlue : new Color(alphaValue, alphaValue, alphaValue, alphaValue);
        }

        public override void AI()
        {

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            Player player = Main.player[NPC.target];
            if (player.dead)
            {

                NPC.position += new Vector2(0, 10);
                NPC.EncourageDespawn(1);
                return;
            }
            DesertBehemothSandSharks.allJump = false;
            if (!isFlyingStage)
                if (!isBurrowing)
                {
                    NPC.noGravity = false;
                    alphaValue += 0.1f;

                }
                else
                {
                    NPC.noGravity = true;
                    alphaValue -= 0.1f;
                }

            alphaValue = MathHelper.Clamp(alphaValue, 0f, 1f);

            if (!player.ZoneDesert)
                NPC.defense = 50;
            else
                NPC.defense = 10;

            if (player.Center.Y > NPC.Center.Y + 32)
                fallThroughPlatforms = true;
            else
                fallThroughPlatforms = false;

            if (NPC.collideX)
            {

                NPC.position.Y -= 16;

            }

            if (NPC.life < NPC.lifeMax / 2 && !isFlyingStage)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/TheBehemothTheme2");
                currentState = attacks.toFlightStage;
                attacking = false;
            }

            if (!isFlyingStage)
            {
                switch (currentState)
                {

                    case attacks.Idle:
                        IdleDuration--;
                        AttackCounter = 0f;
                        AttackTimer = 0f;
                        AttackCooldown = 0f;

                        if (NPC.collideY)
                            NPC.velocity.X = 0f;

                        if (IdleDuration <= 0)
                        {

                            nextAttack++;
                            if (nextAttack == 3)
                                nextAttack = 0;

                            currentState = (attacks)nextAttack;

                        }


                        break;

                    case attacks.Attack1:


                        AttackCooldown++;
                        if (AttackCooldown >= 60)
                        {
                            NPC.velocity = new Vector2(15 * NPC.direction, 0);
                            attacking = true;
                            AttackTimer++;
                            Dust.NewDust(NPC.Center, 50, 25, DustID.SandstormInABottle);


                            if (AttackCooldown == 60)
                            {
                                SoundEngine.PlaySound(SoundID.Item60, NPC.position);
                                if (player.Center.Y < NPC.Center.Y - 100)
                                {
                                    if (NPC.direction == 1)
                                    {
                                        NPC.position = player.Center - new Vector2(500, 32);
                                    }
                                    else
                                    {
                                        NPC.position = player.Center + new Vector2(500, -32);

                                    }
                                    for (int i = 0; i < 30; i++)
                                    {

                                        Vector2 spread = Main.rand.NextVector2CircularEdge(30, 30);


                                        Dust.NewDust(NPC.Center, 50, 25, DustID.AmberBolt, spread.X, spread.Y);


                                    }

                                    AttackTimer = -10;
                                }
                            }





                        }
                        else
                        {
                            LookAtPlayer(player);
                        }

                        if (AttackTimer > 60 && NPC.Center.Distance(player.Center) > 100)
                        {
                            AttackCooldown = 30;
                            AttackTimer = 0;
                            AttackCounter++;
                            if (AttackCounter >= 4)
                            {
                                IdleDuration = 160;
                                currentState = attacks.Idle;

                            }
                            NPC.velocity.X = 0f;
                            attacking = false;
                        }


                        break;

                    case attacks.Attack2:
                        AttackCooldown++;
                        LookAtPlayer(player);
                        if (AttackCooldown == 60)
                        {
                            NPC.velocity.Y = -15;
                            NPC.velocity.X = (NPC.Center.Distance(player.Center) / 150) * NPC.direction;

                            NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -30, 30);
                            attacking = true;
                        }

                        if (attacking && NPC.collideY && AttackCooldown > 80)
                        {
                            isBurrowing = true;



                        }

                        if (isBurrowing)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                Dust.NewDust(NPC.Center, 50, 25, DustID.AmberBolt, Main.rand.Next(-5, 6), -10);
                                Dust.NewDust(NPC.Center, 50, 25, DustID.Sand, Main.rand.Next(-5, 6), -25);


                            }
                            AttackTimer++;
                            if (AttackTimer < 100)
                                NPC.velocity = (player.Center - NPC.Center) / 20;
                            else NPC.velocity = Vector2.Zero;

                            if (AttackTimer == 160 && isBurrowing)
                            {
                                isBurrowing = false;
                                attacking = false;
                                DesertBehemothSandSharks.allJump = true;
                                currentState = attacks.Idle;
                                IdleDuration = 60;
                                NPC.velocity = -Vector2.UnitY * 15;
                                NPC.velocity.X = (NPC.Center.Distance(player.Center) / 150) * NPC.direction;

                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                    for (int i = 0; i < 10; i++)
                                    {
                                        Dust.NewDust(NPC.Center, 50, 25, DustID.AmberBolt, Main.rand.Next(-5, 6), -10);
                                        Dust.NewDust(NPC.Center, 50, 25, DustID.Sand, Main.rand.Next(-5, 6), -25);
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -15).RotatedByRandom(MathHelper.ToRadians(45)), ModContent.ProjectileType<DesertBehemothProjectile>(), 50, 1f, -1, 2);


                                    }

                            }

                        }



                        break;

                    case attacks.Attack3:
                        LookAtPlayer(player);
                        AttackCooldown++;

                        if (AttackCooldown == 30)
                        {
                            IdleDuration = 60;
                            currentState = attacks.Idle;

                            spawnWeakSandsharks();


                        }





                        break;

                    case attacks.toFlightStage:

                        AttackCooldown++;

                        if (AttackCooldown > 60)
                        {
                            AttackCounter = 0f;
                            AttackTimer = 0f;
                            AttackCooldown = 0f;
                            IdleDuration = 60f;
                            isFlyingStage = true;
                            NPC.velocity = Vector2.Zero;
                            currentState = attacks.Idle;
                            NPC.noGravity = true;
                            nextAttack = 0;


                        }

                        break;
                }

            }
            else
            {
                switch (currentState)
                {

                    case attacks.Idle:
                        IdleDuration--;
                        AttackCounter = 0f;
                        AttackTimer = 0f;
                        AttackCooldown = 0f;
                        NPC.velocity *= 0.95f;
                        if (IdleDuration <= 0)
                        {
                            nextAttack++;
                            if (nextAttack >= 3)
                                nextAttack = 0;
                            currentState = (attacks)nextAttack;

                        }
                        break;

                    case attacks.Attack1:
                        AttackCooldown++;
                        NPC.velocity = ((player.Center + new Vector2(0, -450)) - NPC.Center) / 20;
                        LookAtPlayer(player);
                        if (AttackCooldown % 40 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                        {

                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -10), ModContent.ProjectileType<DesertBehemothProjectile>(), 50, 1f, -1, 2);
                            for (int i = 1; i < 3; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -10).RotatedBy(MathHelper.ToRadians(5 * i)), ModContent.ProjectileType<DesertBehemothProjectile>(), 50, 1f, -1, 2);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -10).RotatedBy(MathHelper.ToRadians(5 * i * -1)), ModContent.ProjectileType<DesertBehemothProjectile>(), 50, 1f, -1, 2);


                            }
                        }



                        if ((AttackCooldown == 160 || AttackCooldown == 460 || AttackCooldown == 760) && Main.netMode != NetmodeID.MultiplayerClient)
                        {

                            Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center - new Vector2(500, 32), Vector2.Zero, ProjectileID.SandnadoHostileMark, 50, 1f, -1, 2);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(500, -32), Vector2.Zero, ProjectileID.SandnadoHostileMark, 50, 1f, -1, 2);

                            if (Main.expertMode)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(0, -250), Vector2.Zero, ProjectileID.SandnadoHostileMark, 50, 1f, -1, 2);






                        }


                        if (AttackCooldown == 1000)
                        {
                            IdleDuration = 120;
                            currentState = attacks.Idle;

                        }

                        break;


                    case attacks.Attack2:

                        AttackCooldown++;
                        if (!attacking)
                            NPC.velocity = ((player.Center + new Vector2(0, -450)) - NPC.Center) / 20;

                        if (AttackCooldown > 40)
                        {
                            attacking = true;
                            NPC.velocity.Y += 1;
                            NPC.velocity.X = 0;


                        }

                        if (attacking && (NPC.collideY || AttackCooldown > 80))
                        {
                            AttackTimer++;
                            NPC.velocity = Vector2.Zero;





                            if (AttackTimer % 20 == 0)
                            {
                                AttackCounter++;


                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(255 * AttackCounter, -55), Vector2.Zero, ProjectileID.SandnadoHostileMark, 50, 1f, -1, 2);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(255 * AttackCounter * -1, -55), Vector2.Zero, ProjectileID.SandnadoHostileMark, 50, 1f, -1, 2);


                                }


                                if (AttackCounter % 3 == 0)
                                {
                                    spawnWeakSandsharks();
                                }

                                if (AttackCounter == 9)
                                {
                                    IdleDuration = 60;
                                    currentState = attacks.Idle;
                                    NPC.velocity.Y = -25;
                                    attacking = false;
                                }

                            }

                        }

                        break;


                    case attacks.Attack3:

                        AttackCooldown++;
                        if (AttackCooldown >= 60)
                        {
                            NPC.velocity = new Vector2(15 * NPC.direction, 0);
                            attacking = true;
                            AttackTimer++;
                            Dust.NewDust(NPC.Center, 50, 25, DustID.SandstormInABottle);

                            if (AttackCooldown % 8 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.UnitY, ModContent.ProjectileType<DesertBehemothIndicatorProj>(), 40, 1);
                            }

                            if (AttackCooldown == 60)
                            {
                                NPC.position.Y = player.position.Y - 400;

                                if (AttackCounter == 0)
                                {
                                    if (NPC.direction == 1)
                                    {
                                        NPC.position.X = player.Center.X - 400;
                                    }
                                    else
                                    {
                                        NPC.position.X = player.Center.X + 400;

                                    }
                                }

                                for (int i = 0; i < 30; i++)
                                {

                                    Vector2 spread = Main.rand.NextVector2CircularEdge(15, 15);


                                    Dust.NewDust(NPC.Center, 50, 25, DustID.AmberBolt, spread.X, spread.Y);


                                }
                                SoundEngine.PlaySound(SoundID.Item60, NPC.position);

                            }





                        }
                        else
                        {
                            LookAtPlayer(player);
                        }

                        if (AttackTimer > 60 && Math.Abs(NPC.Center.X - player.Center.X) > 400)
                        {
                            AttackCooldown = 30;
                            AttackTimer = 0;
                            AttackCounter++;
                            if (AttackCounter >= 7)
                            {
                                IdleDuration = 160;
                                currentState = attacks.Idle;

                            }
                            NPC.velocity.X *= 0.1f;
                            attacking = false;

                            if (Main.expertMode && Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center - new Vector2(800, 0), Vector2.UnitX, ModContent.ProjectileType<DesertBehemothIndicatorProj>(), 40, 1);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(800, 0), -Vector2.UnitX, ModContent.ProjectileType<DesertBehemothIndicatorProj>(), 40, 1);

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center - new Vector2(0, 800), Vector2.UnitY, ModContent.ProjectileType<DesertBehemothIndicatorProj>(), 40, 1);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(0, 800), -Vector2.UnitY, ModContent.ProjectileType<DesertBehemothIndicatorProj>(), 40, 1);
                            }
                        }



                        break;
                }
            }



        }


    }


    public class DesertBehemothProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.SandBallFalling;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.aiStyle = -1;


        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.1f;
            Dust.NewDust(Projectile.position, 16, 16, DustID.Sand);
        }


    }

    public class DesertBehemothSandSharks : GlobalNPC
    {

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {

            return entity.type == NPCID.SandShark;
        }

        public override bool InstancePerEntity => true;

        //public override bool? CanFallThroughPlatforms(NPC npc)
        //{
        //    return npc.Center.Y + 32 < Main.player[npc.target].Center.Y;
        //}

        public static bool allJump = false;
        bool stayUntillLanding = false;
        public override bool PreAI(NPC npc)
        {
            Player player = Main.player[npc.target];

            if (NPC.AnyNPCs(ModContent.NPCType<DesertBehemoth>()))
            {
                if (allJump)
                {
                    npc.velocity = (player.Center - npc.Center);
                    npc.velocity.Normalize();
                    npc.velocity *= 15;
                    stayUntillLanding = true;
                }


            }

            if (stayUntillLanding)
            {
                npc.velocity.Y += 0.1f;
                if (npc.collideY)
                    stayUntillLanding = false;
                return false;

            }


            return true;
        }
    }

    public class DesertBehemothIndicatorProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.SandBallFalling;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 2500;
        }
        bool fire = false;
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            fire = false;
            Projectile.rotation = Projectile.ai[0];

            for (int i = 0; i < 35; i++)
            {
                Dust.NewDust(Projectile.Center + Main.rand.NextVector2CircularEdge(25, 25), 0, 0, DustID.YellowStarDust);
            }
        }


        private static Asset<Texture2D> texture;

        public override bool PreDraw(ref Color lightColor)
        {
            texture = TextureAssets.Extra[98];

            Vector2 spawner = Projectile.Center - Main.screenPosition;
            float maxDistance = 1700;
            if (!fire)
                for (float seg = 1; seg < maxDistance; seg++)
                {
                    Main.EntitySpriteDraw(texture.Value, spawner + (seg * 18) * (Projectile.rotation - MathHelper.ToRadians(90)).ToRotationVector2(), new Rectangle(0, 0, 72, 72), Color.Lerp(Color.Yellow, Color.Red, Math.Abs(MathF.Sin(seg * 0.08f + Projectile.timeLeft * 0.08f))), Projectile.rotation, texture.Size() / 2f, 0.75f, SpriteEffects.None);

                }
            Main.EntitySpriteDraw(texture.Value, spawner, new Rectangle(0, 0, 72, 72), Color.AntiqueWhite, Projectile.rotation, texture.Size() / 2f, 2, SpriteEffects.None);

            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {

            return new Color(1f, 1f, 1f, 1f);


        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 25; i++)
            {
                Vector2 circleExplosion = Main.rand.NextVector2CircularEdge(5, 5);
                Dust.NewDustPerfect(Projectile.Center, DustID.AmberBolt, circleExplosion).noGravity = true;

            }
        }

        public override void AI()
        {

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            for (int i = 0; i < 4; i++)
                Dust.NewDustPerfect(Projectile.Center, DustID.AmberBolt).noGravity = true;

            if (Projectile.timeLeft == 250)
            {
                fire = true;
                Projectile.velocity = (Projectile.rotation - MathHelper.ToRadians(90)).ToRotationVector2() * 35;
            }

        }



    }

}