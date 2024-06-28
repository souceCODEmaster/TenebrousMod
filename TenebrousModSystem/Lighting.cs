using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria;
using Terraria.ModLoader;

namespace TenebrousMod.TenebrousModSystem
{
    public static class WeaponLighting
    {
        /// <summary>
        /// LightAmount is how much light there will be.
        /// </summary>
        public static bool LightingOnGround(Item Item, int LightAmount, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Lighting.AddLight(Item.Center, Color.White.ToVector3() * LightAmount);
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            Rectangle frame;

            if (Main.itemAnimations[Item.type] != null)
            {
                frame = Main.itemAnimations[Item.type].GetFrame(texture, Main.itemFrameCounter[whoAmI]);
            }
            else
            {
                frame = texture.Frame();
            }

            Vector2 frameOrigin = frame.Size() / 2f;
            Vector2 offset = new Vector2(Item.width / 2 - frameOrigin.X, Item.height - frame.Height);
            Vector2 drawPos = Item.position - Main.screenPosition + frameOrigin + offset;

            float time = Main.GlobalTimeWrappedHourly;
            float timer = Item.timeSinceItemSpawned / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;

            for (float i = 0f; i < 1f; i += 0.35f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(255, 175, 247, 40), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.30f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 2f).RotatedBy(radians) * time, frame, new Color(255, 155, 241, 55), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public static void PostLighting(Item Item, int LightAmount)
        {
            Lighting.AddLight(Item.Center, Color.White.ToVector3() * LightAmount);
        }
    }
    public class CritterItemLighting : ModSystem
    {
        public static void DropLighting(Item Item)
        {
            Lighting.AddLight(Item.Center, Color.White.ToVector3() * 1f);
        }
    }
}
