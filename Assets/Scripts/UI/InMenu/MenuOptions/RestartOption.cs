using UnityEngine;

namespace UI.InMenu.MenuOptions
{
    public class RestartOption : MenuOptionInterface
    {
        public RestartOption(GameObject optionObject) : base(optionObject, "Restart")
        { }

        public override void OnSelect()
        {
            RoundManager.StartRoundFromIntro();
        }
    }
}