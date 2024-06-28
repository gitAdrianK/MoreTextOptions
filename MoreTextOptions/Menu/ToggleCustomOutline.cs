using JumpKing.PauseMenu.BT.Actions;

namespace MoreTextOptions.Menu
{
    public class ToggleCustomOutline : ITextToggle
    {
        public ToggleCustomOutline() : base(ModEntry.Preferences.IsCustomOutline)
        {
        }

        protected override string GetName() => "Custom outline colour";

        protected override void OnToggle()
        {
            ModEntry.Preferences.IsCustomOutline = !ModEntry.Preferences.IsCustomOutline;
        }
    }
}
