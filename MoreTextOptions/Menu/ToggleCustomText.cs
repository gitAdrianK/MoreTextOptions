using JumpKing.PauseMenu.BT.Actions;

namespace MoreTextOptions.Menu
{
    public class ToggleCustomText : ITextToggle
    {
        public ToggleCustomText() : base(ModEntry.Preferences.IsCustomTextColor)
        {
        }

        protected override string GetName() => "Custom text colour";

        protected override void OnToggle()
        {
            ModEntry.Preferences.IsCustomTextColor = !ModEntry.Preferences.IsCustomTextColor;
        }
    }
}
