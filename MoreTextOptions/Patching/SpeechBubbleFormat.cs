using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using JK = JumpKing.MiscEntities.OldMan;
using Xna = Microsoft.Xna.Framework.Graphics;

namespace MoreTextOptions.Patching
{
    public class SpeechBubbleFormat
    {
        public SpeechBubbleFormat()
        {
            Harmony harmony = ModEntry.Harmony;

            MethodInfo chopString = typeof(JK.SpeechBubbleFormat).GetMethod(nameof(JK.SpeechBubbleFormat.ChopString),
                new Type[] { typeof(string), typeof(Xna.SpriteFont), typeof(int), typeof(char[]) });

            HarmonyMethod removeTags = new HarmonyMethod(typeof(SpeechBubbleFormat).GetMethod(nameof(RemoveTags)));
            harmony.Patch(
                chopString,
                prefix: removeTags);

            HarmonyMethod reinsertTags = new HarmonyMethod(typeof(SpeechBubbleFormat).GetMethod(nameof(ReinsertTags)));
            harmony.Patch(
                chopString,
                postfix: reinsertTags);
        }

        public static bool RemoveTags(out string __state, ref string full_str)
        {
            __state = string.Empty;
            if (ModEntry.REGEX.IsMatch(full_str))
            {
                __state = full_str;
                full_str = ModEntry.REGEX.Replace(full_str, string.Empty);
            }
            return true;
        }

        public static void ReinsertTags(string __state, ref List<string> __result)
        {
            if (__state == string.Empty || __result == null)
            {
                return;
            }

            string fullStr = __state;
            List<string> withTags = new List<string>();

            int index = 0;

            // TODO: Reinsert tags :)
            foreach (string chop in __result)
            {
                Debugger.Log(1, "", "Chop -" + chop + "\n");
                foreach (char c in chop)
                {
                }
            }

            //__result = withTags;
        }
    }
}
