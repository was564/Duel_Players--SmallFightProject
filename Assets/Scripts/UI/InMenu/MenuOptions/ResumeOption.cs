using UnityEngine;

namespace UI.InMenu.MenuOptions
{
    public class ResumeOption : MenuOptionInterface
    {

        public ResumeOption(GameObject optionObject) : base(optionObject, "Resume")
        { }

        public override void OnSelect()
        {
            RoundManager.ResumeGame();
        }
    }
}