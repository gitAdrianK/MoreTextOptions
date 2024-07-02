using JumpKing.PauseMenu.BT.Actions;

namespace MoreTextOptions.Menu
{
    public class ToggleDisableOutline : ITextToggle
    {
        public ToggleDisableOutline() : base(ModEntry.Preferences.IsOutlineDisabled)
        {
        }

        protected override string GetName() => "Disable text outline";

        protected override void OnToggle()
        {
            ModEntry.Preferences.IsOutlineDisabled = !ModEntry.Preferences.IsOutlineDisabled;
        }
    }
}
