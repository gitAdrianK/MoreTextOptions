using HarmonyLib;
using JumpKing;
using JumpKing.Mods;
using JumpKing.PauseMenu;
using JumpKing.Util;
using LessTextOutline.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public static int OffsetX { get; private set; }
        public static int OffsetY { get; private set; }

        [MainMenuItemSetting]
        [PauseMenuItemSetting]
        public static ToggleDisableOutline ToggleOutline(object factory, GuiFormat format)
        {
            return new ToggleDisableOutline();
        }

        [MainMenuItemSetting]
        [PauseMenuItemSetting]
        public static ToggleCustomOutline ToggleCustom(object factory, GuiFormat format)
        {
            return new ToggleCustomOutline();
        }

        [MainMenuItemSetting]
        [PauseMenuItemSetting]
        public static SliderRed SliderRed(object factory, GuiFormat format)
        {
            return new SliderRed();
        }

        [MainMenuItemSetting]
        [PauseMenuItemSetting]
        public static SliderGreen SliderGreen(object factory, GuiFormat format)
        {
            return new SliderGreen();
        }

        [MainMenuItemSetting]
        [PauseMenuItemSetting]
        public static SliderBlue SliderBlue(object factory, GuiFormat format)
        {
            return new SliderBlue();
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

            SpriteFont spriteFont = Game1.instance.contentManager.font.MenuFont;
            Point red = spriteFont.MeasureString("Red").ToPoint();
            Point green = spriteFont.MeasureString("Green").ToPoint();
            Point blue = spriteFont.MeasureString("Blue").ToPoint();
            Point alpha = spriteFont.MeasureString("Alpha").ToPoint();
            OffsetX = Math.Max(red.X, Math.Max(green.X, Math.Max(blue.X, alpha.X)));
            OffsetY = red.Y;
        }

        public static bool DisableOutline(SpriteFont p_font, string p_text, Vector2 p_position, ref bool p_is_outlined)
        {
            if (!p_is_outlined)
            {
                return true;
            }

            if (Preferences.IsEnabled)
            {
                p_is_outlined = false;
                return true;
            }

            if (Preferences.IsCustom)
            {
                p_is_outlined = false;

                Color color = new Color(Preferences.Red, Preferences.Green, Preferences.Blue);
                Game1.spriteBatch.DrawString(p_font, p_text, Vector2.Add(p_position, new Vector2(-1f, -1f)), color);
                Game1.spriteBatch.DrawString(p_font, p_text, Vector2.Add(p_position, new Vector2(-1f, 0f)), color);
                Game1.spriteBatch.DrawString(p_font, p_text, Vector2.Add(p_position, new Vector2(-1f, 1f)), color);
                Game1.spriteBatch.DrawString(p_font, p_text, Vector2.Add(p_position, new Vector2(0f, -1f)), color);
                Game1.spriteBatch.DrawString(p_font, p_text, Vector2.Add(p_position, new Vector2(0f, 1f)), color);
                Game1.spriteBatch.DrawString(p_font, p_text, Vector2.Add(p_position, new Vector2(1f, -1f)), color);
                Game1.spriteBatch.DrawString(p_font, p_text, Vector2.Add(p_position, new Vector2(1f, 0f)), color);
                Game1.spriteBatch.DrawString(p_font, p_text, Vector2.Add(p_position, new Vector2(1f, 1f)), color);
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
