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
                    stringBuilder.Append(element);
                }
                i++;
            }

            text = texts.First();

            Vector2 advancedPosition = new Vector2(position.X + spriteFont.MeasureString(text).X, position.Y);
            for (int j = 1; j < texts.Count; j++)
            {
                int remainingR = Convert.ToInt32(colors[j].Substring(1, 2), 16);
                int remainingG = Convert.ToInt32(colors[j].Substring(3, 2), 16);
                int remainingB = Convert.ToInt32(colors[j].Substring(5, 2), 16);
                float remPercentR = color.R / 255.0f * remainingR;
                float remPercentG = color.G / 255.0f * remainingG;
                float remPercentB = color.B / 255.0f * remainingB;
                Color remainingColor = new Color(
                    (int)remPercentR,
                    (int)remPercentG,
                    (int)remPercentB,
                    color.A);
                Game1.spriteBatch.DrawString(spriteFont, texts[j], advancedPosition, remainingColor);
                advancedPosition.X += spriteFont.MeasureString(texts[j]).X;
            }


            int r = Convert.ToInt32(colors.First().Substring(1, 2), 16);
            int g = Convert.ToInt32(colors.First().Substring(3, 2), 16);
            int b = Convert.ToInt32(colors.First().Substring(5, 2), 16);
            float percentR = color.R / 255.0f * r;
            float percentG = color.G / 255.0f * g;
            float percentB = color.B / 255.0f * b;
            color = new Color(
                (int)percentR,
                (int)percentG,
                (int)percentB,
                color.A);

            return true;
        }
    }
}
