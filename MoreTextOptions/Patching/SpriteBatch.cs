using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MoreTextOptions.Patching
{
    public class SpriteBatch
    {
        private static readonly Regex regex = new Regex("{color=\"(#(?:[0-9a-fA-F]{2}){3})\"}", RegexOptions.IgnoreCase);

        public SpriteBatch()
        {
            Harmony harmony = ModEntry.Harmony;

            // For some reason theres 4 methods DrawString that have a body instead of cascading properly, compiler magic I guess ¯\_(ツ)_/¯
            MethodInfo drawString = typeof(Microsoft.Xna.Framework.Graphics.SpriteBatch).GetMethod(nameof(Microsoft.Xna.Framework.Graphics.SpriteBatch.DrawString),
                new Type[] { typeof(SpriteFont), typeof(string), typeof(Vector2), typeof(Color) });
            //MethodInfo drawString2 = typeof(Microsoft.Xna.Framework.Graphics.SpriteBatch).GetMethod(nameof(Microsoft.Xna.Framework.Graphics.SpriteBatch.DrawString),
            //    new Type[] { typeof(SpriteFont), typeof(string), typeof(Vector2), typeof(Color), typeof(float), typeof(Vector2), typeof(Vector2), typeof(SpriteEffect), typeof(float) });
            //MethodInfo drawString3 = typeof(Microsoft.Xna.Framework.Graphics.SpriteBatch).GetMethod(nameof(Microsoft.Xna.Framework.Graphics.SpriteBatch.DrawString),
            //    new Type[] { typeof(SpriteFont), typeof(StringBuilder), typeof(Vector2), typeof(Color) });
            //MethodInfo drawString4 = typeof(Microsoft.Xna.Framework.Graphics.SpriteBatch).GetMethod(nameof(Microsoft.Xna.Framework.Graphics.SpriteBatch.DrawString),
            //    new Type[] { typeof(SpriteFont), typeof(StringBuilder), typeof(Vector2), typeof(Color), typeof(float), typeof(Vector2), typeof(Vector2), typeof(SpriteEffect), typeof(float) });

            HarmonyMethod modifyText = new HarmonyMethod(typeof(SpriteBatch).GetMethod(nameof(ModifyText)));

            harmony.Patch(
                drawString,
                prefix: modifyText);
        }

        public static bool ModifyText(SpriteFont spriteFont, ref string text, ref Vector2 position, ref Color color)
        {
            if (!ContainsTag(text.Trim()))
            {
                return true;
            }

            string[] substrings = regex.Split(text);

            // [0] is an empty string

            int r = Convert.ToInt32(substrings[1].Substring(1, 2), 16);
            int g = Convert.ToInt32(substrings[1].Substring(3, 2), 16);
            int b = Convert.ToInt32(substrings[1].Substring(5, 2), 16);
            Color newColor = new Color(r, g, b, color.A);

            string cleanText = substrings[2];

            float centerX = position.X + spriteFont.MeasureString(text).X / 2;
            color = newColor;
            text = cleanText;
            position = new Vector2(centerX - spriteFont.MeasureString(text).X / 2, position.Y);

            return true;
        }

        private static bool ContainsTag(string text)
        {
            return regex.IsMatch(text);
        }
    }
}
