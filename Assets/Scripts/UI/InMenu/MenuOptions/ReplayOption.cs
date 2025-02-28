using UnityEngine;

namespace UI.InMenu.MenuOptions
{
    public class ReplayOption : MenuOptionInterface
    {

        public ReplayOption(GameObject optionObject) : base(optionObject, "Replay")
        { }

        public override void OnSelect()
        {
            RoundManager.Replay();
        }
    }
}