using HarmonyLib;
using System;
using System.Collections.Generic;
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

            string stringFull = __state;
            int indexFull = 0;

            int start = 0;

            /* Fix for line splits losing colour, doesn't work yet as the SayLine sample does not get changed.
            string currColor = string.Empty;
            string nextColor = string.Empty;
            */

            List<string> withTags = new List<string>();

            foreach (string chop in __result)
            {
                int length = 0;
                // Assuming if we run into a tag the next char is the char we are looking for
                foreach (char c in chop)
                {
                    // The char is the char we are looking for.
                    if (stringFull[indexFull] == c)
                    {
                        length++;
                        indexFull++;
                        continue;
                    }
                    // The char might be the beginning of a tag.
                    if (stringFull[indexFull] == '{'
                        && stringFull.Length - (indexFull + 17) > 0
                        && ModEntry.REGEX.IsMatch(stringFull.Substring(indexFull, 17)))
                    {
                        /* Fix for line splits losing colour p2.
                        // We are at the beginning of the chop.
                        if (length == 0)
                        {
                            currColor = string.Empty;
                        }
                        nextColor = stringFull.Substring(indexFull, 17);
                        */
                        length += 18;
                        indexFull += 18;
                    }
                }
                /* Fix for line splits losing colour p3.
                withTags.Add($"{currColor}{stringFull.Substring(start, length).TrimStart()}");
                currColor = nextColor;
                */
                withTags.Add(stringFull.Substring(start, length));
                start = indexFull;
            }
            __result = withTags;
        }
    }
}
