using JumpKing.PauseMenu.BT.Actions;

namespace LessTextOutline
{
    public class ToggleDisableOutline : ITextToggle
    {
        public ToggleDisableOutline() : base(ModEntry.Preferences.IsEnabled)
        {
        }

        protected override string GetName() => "Disable text outline";

        protected override void OnToggle()
        {
            ModEntry.Preferences.IsEnabled = !ModEntry.Preferences.IsEnabled;
        }
    }
}
