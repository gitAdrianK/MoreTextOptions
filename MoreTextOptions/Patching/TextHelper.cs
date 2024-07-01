using HarmonyLib;
using JumpKing;
using Microsoft.Xna.Framework;
using System.Reflection;
using JK = JumpKing.Util;
using Xna = Microsoft.Xna.Framework.Graphics;

namespace MoreTextOptions.Patching
{
    public class TextHelper
    {
        public TextHelper()
        {
            Harmony harmony = ModEntry.Harmony;

            MethodInfo drawString = typeof(JK.TextHelper).GetMethod(nameof(JK.TextHelper.DrawString));
            HarmonyMethod modifyText = new HarmonyMethod(typeof(TextHelper).GetMethod(nameof(ModifyText)));
            harmony.Patch(
                drawString,
                prefix: modifyText);
        }

        public static bool ModifyText(Xna.SpriteFont p_font, string p_text, Vector2 p_position, ref Color p_color, ref bool p_is_outlined)
        {
            if (!p_is_outlined)
            {
                return true;
            }

            if (ModEntry.Preferences.IsCustomTextColor)
            {
                p_color = new Color(ModEntry.Preferences.TextRed,
                    ModEntry.Preferences.TextGreen,
                    ModEntry.Preferences.TextBlue);
            }

            if (ModEntry.Preferences.IsOutlineEnabled)
            {
                p_is_outlined = false;
                return true;
            }

            if (ModEntry.Preferences.IsCustomOutline)
            {
                p_is_outlined = false;

                Color color = new Color(ModEntry.Preferences.OutlineRed,
                    ModEntry.Preferences.OutlineGreen,
                    ModEntry.Preferences.OutlineBlue);
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
    }
}
