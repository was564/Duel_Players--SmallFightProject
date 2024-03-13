using UnityEngine;

namespace UI.InMenu.MenuOptions
{
    public class SaveRoundOption : MenuOptionInterface
    {

        public SaveRoundOption(GameObject optionObject) : base(optionObject, "SaveRound")
        {
        }

        public override void OnSelect()
        {
            RoundManager.ResumeGame();
            RoundManager.SaveRoundUntilNow();
        }
    }
}