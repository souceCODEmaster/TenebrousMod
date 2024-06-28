using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using System.Reflection;
using System;
using System.Collections.Generic;
using TenebrousMod.Assets.Textures;
using Terraria.ID;

namespace TenebrousMod.TenebrousModSystem
{

    public class TModMenu : ModMenu
    {
        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("TenebrousMod/Assets/Textures/ModLogo");
        public override Asset<Texture2D> SunTexture => Asset<Texture2D>.Empty;
        public override ModSurfaceBackgroundStyle MenuBackgroundStyle { get => ModContent.GetInstance<Background>(); }
        public override Asset<Texture2D> MoonTexture => Asset<Texture2D>.Empty;



        public override int Music => MusicLoader.GetMusicSlot("TenebrousMod/Assets/Music/MainMenuMusic");
        public override void Load()
        {
            Main.menuBGChangedDay = true;
            base.Load();
        }

        public override string DisplayName => "Tenebrous Theme Day";

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            drawColor = Color.White;
            return base.PreDrawLogo(spriteBatch, ref logoDrawCenter, ref logoRotation, ref logoScale, ref drawColor);
        }




    }

    public class TModMenu2 : ModMenu
    {
        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("TenebrousMod/Assets/Textures/ModLogo");
        public override Asset<Texture2D> SunTexture => Asset<Texture2D>.Empty;
        public override ModSurfaceBackgroundStyle MenuBackgroundStyle { get => ModContent.GetInstance<Background2>(); }
        public override Asset<Texture2D> MoonTexture => Asset<Texture2D>.Empty;



        public override int Music => MusicLoader.GetMusicSlot("TenebrousMod/Assets/Music/MainMenuMusic2");
        public override void Load()
        {
            Main.menuBGChangedDay = true;
            base.Load();
        }
        public override string DisplayName => "Tenebrous Theme Night";

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            drawColor = Color.White;
            return base.PreDrawLogo(spriteBatch, ref logoDrawCenter, ref logoRotation, ref logoScale, ref drawColor);
        }




    }
    public class TModMenu3 : ModMenu
    {
        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("TenebrousMod/Assets/Textures/ModLogo");
        public override Asset<Texture2D> SunTexture => Asset<Texture2D>.Empty;
        public override ModSurfaceBackgroundStyle MenuBackgroundStyle { get => ModContent.GetInstance<Background3>(); }
        public override Asset<Texture2D> MoonTexture => Asset<Texture2D>.Empty;

        private float SizeTimer = 2f; // Start at 2
        private bool isIncreasing = true; // Flag to track the direction of change
        private float orbitRotationPurple = MathHelper.Pi; // Initial angle for the purple moon (180 degrees)
        private float orbitRotationRed = 0f; // Initial angle for the red moon (0 degrees)

        public override void Update(bool isOnTitleScreen)
        {
            if (isIncreasing)
            {
                SizeTimer += 0.0025f;
                if (SizeTimer >= 1.5f)
                {
                    SizeTimer = 1.5f;
                    isIncreasing = false;
                }
            }
            else
            {
                SizeTimer -= 0.0025f;
                if (SizeTimer <= 1f)
                {
                    SizeTimer = 1f;
                    isIncreasing = true;
                }
            }

            // Increment the rotation angles for the orbiting textures
            orbitRotationPurple += 0.005f;
            orbitRotationRed += 0.006f; // Slightly faster increment for the red moon

            // Setting the game time to day
            Main.dayTime = true;
            Main.time = 40000;
        }

        public override int Music => MusicLoader.GetMusicSlot("TenebrousMod/Assets/Music/MagmaRuins");

        public override void Load()
        {
            Main.menuBGChangedDay = true;
            base.Load();
        }

        public override string DisplayName => "???INFERNED MOON???";

        private float rotation;
        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            drawColor = Color.White;

            // Central moon (orange and yellow)
            Texture2D InfernedMoon = ModContent.Request<Texture2D>("TenebrousMod/Assets/Textures/InfernedMoon").Value;
            // Small moon in back (purple)
            Texture2D InfernedMoon2 = ModContent.Request<Texture2D>("TenebrousMod/Assets/Textures/InfernedMoon2").Value;
            // Small moon in front (red)
            Texture2D InfernedMoon3 = ModContent.Request<Texture2D>("TenebrousMod/Assets/Textures/InfernedMoon3").Value;

            Vector2 logoDrawPos = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            rotation += 0.002f;

            // Calculate the orbiting positions
            float orbitRadius = 100f; // Radius of the orbit
            Vector2 orbitPosPurple = logoDrawPos + new Vector2(
                orbitRadius * (float)Math.Cos(orbitRotationPurple),
                orbitRadius * (float)Math.Sin(orbitRotationPurple));

            Vector2 orbitPosRed = logoDrawPos + new Vector2(
                orbitRadius * (float)Math.Cos(orbitRotationRed),
                orbitRadius * (float)Math.Sin(orbitRotationRed));

            // Draw the first smaller moon behind the larger moon
            spriteBatch.Draw(
                             InfernedMoon2, // The texture being drawn
                             orbitPosPurple, // The position of the texture
                             new Rectangle(0, 0, InfernedMoon2.Width, InfernedMoon2.Height),
                             Color.White, // The color of the texture
                             -rotation, // The rotation of the texture (opposite direction for visual effect)
                             InfernedMoon2.Size() * 0.5f, // The centerpoint of the texture
                             SizeTimer * 0.6f, // The scale of the texture (smaller for orbiting effect)
                             SpriteEffects.None,
                             0f);

            // Draw the central InfernedMoon texture
            spriteBatch.Draw(
                             InfernedMoon, // The texture being drawn
                             logoDrawPos, // The position of the texture
                             new Rectangle(0, 0, InfernedMoon.Width, InfernedMoon.Height),
                             Color.White, // The color of the texture
                             rotation, // The rotation of the texture
                             InfernedMoon.Size() * 0.5f, // The centerpoint of the texture
                             SizeTimer, // The scale of the texture
                             SpriteEffects.None,
                             0f);

            // Draw the second smaller moon in front of the larger moon
            spriteBatch.Draw(
                             InfernedMoon3, // The texture being drawn
                             orbitPosRed, // The position of the texture
                             new Rectangle(0, 0, InfernedMoon3.Width, InfernedMoon3.Height),
                             Color.White, // The color of the texture
                             -rotation, // The rotation of the texture (opposite direction for visual effect)
                             InfernedMoon3.Size() * 0.5f, // The centerpoint of the texture
                             SizeTimer * 0.6f, // The scale of the texture (smaller for orbiting effect)
                             SpriteEffects.None,
                             0f);

            return true;
        }
    }
}

