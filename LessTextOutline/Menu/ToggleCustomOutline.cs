using JumpKing.PauseMenu.BT.Actions;

namespace LessTextOutline.Menu
{
    public class ToggleCustomOutline : ITextToggle
    {
        public ToggleCustomOutline() : base(ModEntry.Preferences.IsCustom)
        {
        }

        protected override string GetName() => "Custom outline colour";

        protected override void OnToggle()
        {
            ModEntry.Preferences.IsCustom = !ModEntry.Preferences.IsCustom;
        }
    }
}
