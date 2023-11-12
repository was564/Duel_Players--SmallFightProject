using UnityEngine;

namespace UI.InMenu.MenuOptions
{
    public class RestartOption : MenuOptionInterface
    {
        private GameRoundManager _gameManager;
        private MenuInGame _menuManger;
        
        public RestartOption(GameObject optionObject) : base(optionObject, "Restart")
        {
            _gameManager = GameObject.FindObjectOfType<GameRoundManager>();
            _menuManger = GameObject.FindObjectOfType<MenuInGame>();
        }

        public override void OnSelect()
        {
            _gameManager.StartRound();
            _menuManger.ControlMenu();
        }
    }
}