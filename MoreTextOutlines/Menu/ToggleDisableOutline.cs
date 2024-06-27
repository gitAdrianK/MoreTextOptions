using JumpKing.PauseMenu.BT.Actions;

namespace MoreTextOptions.Menu
{
    public class ToggleDisableOutline : ITextToggle
    {
        public ToggleDisableOutline() : base(ModEntry.Preferences.IsOutlineEnabled)
        {
        }

        protected override string GetName() => "Disable text outline";

        protected override void OnToggle()
        {
            ModEntry.Preferences.IsOutlineEnabled = !ModEntry.Preferences.IsOutlineEnabled;
        }
    }
}
