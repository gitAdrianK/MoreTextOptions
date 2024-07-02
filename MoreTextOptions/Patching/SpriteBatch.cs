using HarmonyLib;
using JumpKing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

        public static bool ModifyText(Xna.SpriteFont spriteFont, ref string text, ref Vector2 position, ref Color color)
        {
            if (!ModEntry.REGEX.IsMatch(text))
            {
                // Genuinely don't know why just allowing the original would mess with alpha
                int ogR = Math.Min(color.R, color.A);
                int ogG = Math.Min(color.G, color.A);
                int ogB = Math.Min(color.B, color.A);
                color = new Color(ogR, ogG, ogB, color.A);
                return true;
            }

            string[] substrings = ModEntry.REGEX.Split(text);

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

            List<string> colors = new List<string>();
            List<string> texts = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();
            int i = -1;
            foreach (string element in pairs)
            {
                i++;
                if (i % 2 == 0)
                {
                    colors.Add(element);
                    continue;
                }
                texts.Add(element);
                stringBuilder.Append(element);
            }

            int r = Math.Min(Convert.ToInt32(colors.First().Substring(1, 2), 16), color.A);
            int g = Math.Min(Convert.ToInt32(colors.First().Substring(3, 2), 16), color.A);
            int b = Math.Min(Convert.ToInt32(colors.First().Substring(5, 2), 16), color.A);
            color = new Color(r, g, b, color.A);

            text = texts.First();

            Vector2 advancedPosition = new Vector2(position.X + spriteFont.MeasureString(text).X, position.Y);
            for (int j = 1; j < texts.Count; j++)
            {
                int remainingR = Math.Min(Convert.ToInt32(colors[j].Substring(1, 2), 16), color.A);
                int remainingG = Math.Min(Convert.ToInt32(colors[j].Substring(3, 2), 16), color.A);
                int remainingB = Math.Min(Convert.ToInt32(colors[j].Substring(5, 2), 16), color.A);
                Color remainingColor = new Color(remainingR, remainingG, remainingB, color.A);
                Game1.spriteBatch.DrawString(spriteFont, texts[j], advancedPosition, remainingColor);
                advancedPosition.X += spriteFont.MeasureString(texts[j]).X;
            }

            return true;
        }
    }
}
