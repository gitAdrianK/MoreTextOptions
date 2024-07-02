using BehaviorTree;
using HarmonyLib;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using JK = JumpKing.MiscEntities.OldMan;

namespace MoreTextOptions.Patching
{
    public class SayLine
    {
        // Basically same regex, except it requires the tag to be the first thing in the string
        private static readonly Regex PREFIX_REGEX = new Regex("^{color=\"(#(?:[0-9a-fA-F]{2}){3})\"}", RegexOptions.IgnoreCase);

        // We need this workaround as the timer inside the method MyRun gets reset before we run our patch
        private static int prevLength = 0;

        public SayLine()
        {
            Harmony harmony = ModEntry.Harmony;

            MethodInfo sayLineMyRun = typeof(JK.SayLine).GetMethod("MyRun", BindingFlags.NonPublic | BindingFlags.Instance);
            HarmonyMethod addFullTagToCurrentLine = new HarmonyMethod(typeof(SayLine).GetMethod(nameof(AddFullTagToCurrentLine)));
            harmony.Patch(
                sayLineMyRun,
                postfix: addFullTagToCurrentLine);

            MethodInfo resetLine = typeof(JK.SayLine).GetMethod("Reset", BindingFlags.NonPublic | BindingFlags.Instance);
            HarmonyMethod resetLength = new HarmonyMethod(typeof(SayLine).GetMethod(nameof(ResetLine)));
            harmony.Patch(
                resetLine,
                postfix: resetLength);
        }

        public static void AddFullTagToCurrentLine(SayLine __instance, BTresult __result)
        {
            if (__result == BTresult.Success)
            {
                prevLength = 0;
                return;
            }

            Traverse instance = Traverse.Create(__instance);
            Traverse traverseCurrentLine = instance.Field("m_current_line");

            string currentLine = traverseCurrentLine.GetValue<string>();
            if (currentLine.Length == prevLength || currentLine.Last() != '{')
            {
                return;
            }

            Traverse traverseSampleLine = instance.Field("m_sample_line");
            string sampleLine = traverseSampleLine.GetValue<string>();
            string afterCurrent = sampleLine.Substring(currentLine.Length - 1, sampleLine.Length - currentLine.Length);
            if (!PREFIX_REGEX.IsMatch(afterCurrent))
            {
                return;
            }

            // The regex is 17 characters long, '{' is already added (16)
            // but we are also adding the next character (17).
            traverseCurrentLine.SetValue(currentLine + afterCurrent.Substring(1, 17));
            prevLength += 17;
        }

        public static void ResetLine()
        {
            prevLength = 0;
        }
    }
}
