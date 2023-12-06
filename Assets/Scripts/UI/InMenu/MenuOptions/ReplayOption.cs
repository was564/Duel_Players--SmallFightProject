using UnityEngine;

namespace UI.InMenu.MenuOptions
{
    public class ReplayOption : MenuOptionInterface
    {
        private GameRoundManager _gameManager;
        private MenuInGame _menuManager;

        public ReplayOption(GameObject optionObject) : base(optionObject, "Replay")
        {
            _gameManager = GameObject.FindObjectOfType<GameRoundManager>();
            _menuManager = GameObject.FindObjectOfType<MenuInGame>();
        }

        public override void OnSelect()
        {
            _menuManager.ControlMenu();
            _gameManager.Replay();
        }
    }
}