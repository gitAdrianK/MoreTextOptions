using HarmonyLib;
using JumpKing.Mods;
using JumpKing.PauseMenu;
using JumpKing.Util;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace LessTextOutline
{
    [JumpKingMod("Zebra.LessTextOutline")]
    public static class ModEntry
    {
        const string IDENTIFIER = "Zebra.LessTextOutline";
        const string HARMONY_IDENTIFIER = "Zebra.LessTextOutline.Harmony";
        const string SETTINGS_FILE = "Zebra.LessTextOutline.Settings.xml";

        private static string AssemblyPath { get; set; }
        public static Preferences Preferences { get; private set; }

        [MainMenuItemSetting]
        [PauseMenuItemSetting]
        public static ToggleDisableOutline Toggle(object factory, GuiFormat format)
        {
            return new ToggleDisableOutline();
        }

        /// <summary>
        /// Called by Jump King before the level loads
        /// </summary>
        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
            AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            try
            {
                Preferences = XmlSerializerHelper.Deserialize<Preferences>($@"{AssemblyPath}\{SETTINGS_FILE}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[ERROR] [{IDENTIFIER}] {e.Message}");
                Preferences = new Preferences();
            }
            Preferences.PropertyChanged += SaveSettingsOnFile;

            Harmony harmony = new Harmony(HARMONY_IDENTIFIER);
            MethodInfo drawString = typeof(TextHelper).GetMethod(nameof(TextHelper.DrawString));
            HarmonyMethod disableOutline = new HarmonyMethod(typeof(ModEntry).GetMethod(nameof(DisableOutline)));
            harmony.Patch(
                drawString,
                prefix: disableOutline);
        }

        public static bool DisableOutline(ref bool p_is_outlined)
        {
            if (Preferences.IsEnabled)
            {
                p_is_outlined = false;
            }
            return true;
        }

        private static void SaveSettingsOnFile(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            try
            {
                XmlSerializerHelper.Serialize($@"{AssemblyPath}\{SETTINGS_FILE}", Preferences);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[ERROR] [{IDENTIFIER}] {e.Message}");
            }
        }
    }
}
