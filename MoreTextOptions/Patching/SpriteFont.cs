using HarmonyLib;
using System;
using System.Reflection;
using Xna = Microsoft.Xna.Framework.Graphics;

namespace MoreTextOptions.Patching
{
    public class SpriteFont
    {
        public SpriteFont()
        {
            Harmony harmony = ModEntry.Harmony;

            MethodInfo measureString = typeof(Xna.SpriteFont).GetMethod(nameof(Xna.SpriteFont.MeasureString),
                new Type[] { typeof(string) });
            HarmonyMethod removeTags = new HarmonyMethod(typeof(SpriteFont).GetMethod(nameof(RemoveTags)));
            harmony.Patch(
                measureString,
                prefix: removeTags);
        }

        public static bool RemoveTags(ref string text)
        {
            text = ModEntry.REGEX.Replace(text, string.Empty);
            return true;
        }
    }
}