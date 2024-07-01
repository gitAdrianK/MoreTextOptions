using HarmonyLib;
using System;
using System.Reflection;

namespace MoreTextOptions.Patching
{
    public class SpriteFont
    {
        public SpriteFont()
        {
            Harmony harmony = ModEntry.Harmony;

            MethodInfo measureString = typeof(Microsoft.Xna.Framework.Graphics.SpriteFont).GetMethod(nameof(Microsoft.Xna.Framework.Graphics.SpriteFont.MeasureString),
                new Type[] { typeof(string) });
            HarmonyMethod removeTags = new HarmonyMethod(typeof(SpriteBatch).GetMethod(nameof(RemoveTags)));
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