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

            Preferences pref = ModEntry.Preferences;

            if (pref.IsCustomTextColor)
            {
                p_color = new Color(pref.TextRed, pref.TextGreen, pref.TextBlue, p_color.A);
            }

            if (pref.IsOutlineDisabled)
            {
                p_is_outlined = false;
                return true;
            }

            if (pref.IsCustomOutline)
            {
                p_is_outlined = false;

                Color color = new Color(pref.OutlineRed, pref.OutlineGreen, pref.OutlineBlue, p_color.A);
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
