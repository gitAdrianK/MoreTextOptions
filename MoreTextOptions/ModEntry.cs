using HarmonyLib;
using JumpKing;
using JumpKing.Mods;
using JumpKing.PauseMenu;
using JumpKing.PauseMenu.BT;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoreTextOptions.Menu;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MoreTextOptions
{
    [JumpKingMod(IDENTIFIER)]
    public static class ModEntry
    {
        const string IDENTIFIER = "Zebra.MoreTextOptions";
        const string HARMONY_IDENTIFIER = "Zebra.MoreTextOptions.Harmony";
        const string SETTINGS_FILE = "Zebra.MoreTextOptions.Settings.xml";

        public static readonly Regex REGEX = new Regex("{color=\"(#(?:[0-9a-fA-F]{2}){3})\"}", RegexOptions.IgnoreCase);
        public static Harmony Harmony { get; set; }
        private static string AssemblyPath { get; set; }
        public static Preferences Preferences { get; private set; }
        public static int OffsetX { get; private set; }
        public static int OffsetY { get; private set; }

        [MainMenuItemSetting]
        public static TextInfo HintOptionsLocation(object factory, GuiFormat format)
        {
            return new TextInfo("Options in the pausemenu!", Color.Lime);
        }

        // "Let's capitalize function names" -Some genius

        [PauseMenuItemSetting]
        public static ToggleCustomText ToggleCustomText(object factory, GuiFormat format)
        {
            return new ToggleCustomText();
        }

        [PauseMenuItemSetting]
        public static SliderTextRed SliderTextRed(object factory, GuiFormat format)
        {
            return new SliderTextRed();
        }

        [PauseMenuItemSetting]
        public static SliderTextGreen SliderTextGreen(object factory, GuiFormat format)
        {
            return new SliderTextGreen();
        }

        [PauseMenuItemSetting]
        public static SliderTextBlue SliderTextBlue(object factory, GuiFormat format)
        {
            return new SliderTextBlue();
        }

        [PauseMenuItemSetting]
        public static ToggleDisableOutline ToggleOutline(object factory, GuiFormat format)
        {
            return new ToggleDisableOutline();
        }

        [PauseMenuItemSetting]
        public static ToggleCustomOutline ToggleCustomOutline(object factory, GuiFormat format)
        {
            return new ToggleCustomOutline();
        }

        [PauseMenuItemSetting]
        public static SliderOutlineRed SliderOutlineRed(object factory, GuiFormat format)
        {
            return new SliderOutlineRed();
        }

        [PauseMenuItemSetting]
        public static SliderOutlineGreen SliderOutlineGreen(object factory, GuiFormat format)
        {
            return new SliderOutlineGreen();
        }

        [PauseMenuItemSetting]
        public static SliderOutlineBlue SliderOutlineBlue(object factory, GuiFormat format)
        {
            return new SliderOutlineBlue();
        }

        /// <summary>
        /// Called by Jump King before the level loads
        /// </summary>
        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
            //Debugger.Launch();

            // BUG: Fading text goes back to white

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

            Harmony = new Harmony(HARMONY_IDENTIFIER);
            new Patching.SayLine();
            new Patching.SpeechBubbleFormat();
            new Patching.SpriteBatch();
            new Patching.SpriteFont();
            new Patching.TextHelper();

            SpriteFont spriteFont = Game1.instance.contentManager.font.MenuFont;
            Point red = spriteFont.MeasureString("Red").ToPoint();
            Point green = spriteFont.MeasureString("Green").ToPoint();
            Point blue = spriteFont.MeasureString("Blue").ToPoint();
            OffsetX = Math.Max(red.X, Math.Max(green.X, blue.X));
            OffsetY = red.Y;
        }

        public static bool ModifyText(SpriteFont p_font, string p_text, Vector2 p_position, ref Color p_color, ref bool p_is_outlined)
        {
            if (!p_is_outlined)
            {
                return true;
            }

            if (Preferences.IsCustomTextColor)
            {
                p_color = new Color(Preferences.TextRed, Preferences.TextGreen, Preferences.TextBlue);
            }

            if (Preferences.IsOutlineEnabled)
            {
                p_is_outlined = false;
                return true;
            }

            if (Preferences.IsCustomOutline)
            {
                p_is_outlined = false;

                Color color = new Color(Preferences.OutlineRed, Preferences.OutlineGreen, Preferences.OutlineBlue);
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
