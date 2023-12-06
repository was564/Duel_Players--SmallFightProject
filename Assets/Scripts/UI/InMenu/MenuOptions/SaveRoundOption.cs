using UnityEngine;

namespace UI.InMenu.MenuOptions
{
    public class SaveRoundOption : MenuOptionInterface
    {
        private GameRoundManager _gameManager;
        private MenuInGame _menuManager;

        public SaveRoundOption(GameObject optionObject) : base(optionObject, "SaveRound")
        {
            _gameManager = GameObject.FindObjectOfType<GameRoundManager>();
            _menuManager = GameObject.FindObjectOfType<MenuInGame>();
        }

        public override void OnSelect()
        {
            _menuManager.ControlMenu();
            _gameManager.SaveRoundUntilNow();
        }
    }
}