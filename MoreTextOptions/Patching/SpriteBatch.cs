using HarmonyLib;
using JumpKing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xna = Microsoft.Xna.Framework.Graphics;

namespace MoreTextOptions.Patching
{
    public class SpriteBatch
    {
        public SpriteBatch()
        {
            Harmony harmony = ModEntry.Harmony;

            MethodInfo drawString = typeof(Xna.SpriteBatch).GetMethod(nameof(Xna.SpriteBatch.DrawString),
                new Type[] { typeof(Xna.SpriteFont), typeof(string), typeof(Vector2), typeof(Color) });
            HarmonyMethod modifyText = new HarmonyMethod(typeof(SpriteBatch).GetMethod(nameof(ModifyText)));
            harmony.Patch(
                drawString,
                prefix: modifyText);
        }

        public static bool ModifyText(Xna.SpriteFont spriteFont, ref string text, Vector2 position, ref Color color)
        {
            if (!ModEntry.REGEX.IsMatch(text))
            {
                return true;
            }

            LinkedList<string> pairs = Sanitize(ModEntry.REGEX.Split(text));


            List<string> colors = new List<string>();
            List<string> texts = new List<string>();
            int i = 0;
            foreach (string element in pairs)
            {
                if (i % 2 == 0)
                {
                    colors.Add(element);
                }
                else
                {
                    texts.Add(element);
                }
                i++;
            }

            if (colors.Count != texts.Count)
            {
                return true;
            }

            text = texts.First();

            Vector2 advancedPosition = new Vector2(position.X + spriteFont.MeasureString(text).X, position.Y);
            for (int j = 1; j < texts.Count; j++)
            {
                Color remainingColor = ColorFromHex(colors[j]);
                remainingColor = AdjustRgbaFromRef(remainingColor, color);
                Game1.spriteBatch.DrawString(spriteFont, texts[j], advancedPosition, remainingColor);
                advancedPosition.X += spriteFont.MeasureString(texts[j]).X;
            }

            Color newColor = ColorFromHex(colors.First());
            newColor = AdjustRgbaFromRef(newColor, color);
            color = newColor;

            return true;
        }

        private static LinkedList<string> Sanitize(string[] substrings)
        {
            LinkedList<string> pairs;
            if (substrings[0] == string.Empty)
            {
                pairs = new LinkedList<string>(substrings.Skip(1));
            }
            else
            {
                pairs = new LinkedList<string>(substrings);
                pairs.AddFirst("#FFFFFF");
            }
            return pairs;
        }

        private static Color ColorFromHex(string hex)
        {
            int r = Convert.ToInt32(hex.Substring(1, 2), 16);
            int g = Convert.ToInt32(hex.Substring(3, 2), 16);
            int b = Convert.ToInt32(hex.Substring(5, 2), 16);
            return new Color(r, g, b);
        }

        private static Color AdjustRgbaFromRef(Color color, Color reference)
        {
            float adjustedR = reference.R / 255.0f * color.R;
            float adjustedG = reference.G / 255.0f * color.G;
            float adjustedB = reference.B / 255.0f * color.B;
            return new Color(
                (int)adjustedR,
                (int)adjustedG,
                (int)adjustedB,
                reference.A);
        }
    }
}
