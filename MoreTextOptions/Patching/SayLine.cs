using HarmonyLib;
using System.Linq;
using System.Reflection;

namespace MoreTextOptions.Patching
{
    public class SayLine
    {
        public SayLine()
        {
            Harmony harmony = ModEntry.Harmony;

            MethodInfo sayLineMyRun = typeof(JumpKing.MiscEntities.OldMan.SayLine).GetMethod("MyRun", BindingFlags.NonPublic | BindingFlags.Instance);
            HarmonyMethod addFullTagToCurrentLine = new HarmonyMethod(typeof(SayLine).GetMethod(nameof(AddFullTagToCurrentLine)));
            harmony.Patch(
                sayLineMyRun,
                postfix: addFullTagToCurrentLine);
        }

        public static void AddFullTagToCurrentLine(SayLine __instance)
        {
            Traverse traverseSampleLine = Traverse.Create(__instance).Field("m_sample_line");
            Traverse traverseCurrentLine = Traverse.Create(__instance).Field("m_current_line");
            string currentLine = traverseCurrentLine.GetValue<string>();
            if (currentLine.Length > 0 && currentLine.Last() != '{')
            {
                return;
            }
            string sampleLine = traverseSampleLine.GetValue<string>();
            string afterCurrent = sampleLine.Substring(currentLine.Length, sampleLine.Length - currentLine.Length);
            if (ModEntry.REGEX.IsMatch(afterCurrent))
            {
                // The regex is 18 characters long
                traverseCurrentLine.SetValue(currentLine + afterCurrent.Substring(0, 18));
            }
        }
    }
}
