using UnityEngine;

namespace GameRound
{
    public class GameRoundPausingState : GameRoundStateInterface
    {
        private MenuInGame _menuManager;

        public GameRoundPausingState(GameRoundStateManager manager)
            : base(manager, GameRoundManager.GameState.Pause)
        {
            _menuManager = GameObject.FindObjectOfType<MenuInGame>();
        }

        public override void Enter()
        {
            RoundManager.ApplySettingInStateByPausing(true);
            PlayersControlManager.BlockAllPlayersInput();
            _menuManager.OnEnableMenu();
        }
        
        public override void Update()
        {
            if(InputManager.IsPressedMenuKey())
            {
                RoundStateManager.ChangeState(GameRoundManager.GameState.NormalPlay);
                return;
            }

            if (InputManager.IsPressedMenuEnterKey())
            {
                _menuManager.SelectMenuOption();
            }
            else
            {
                short result = InputManager.GetPressedMoveKey();
                switch (result)
                {
                    case 1:
                        _menuManager.MenuScrollUp();
                        break;
                    case -1:
                        _menuManager.MenuScrollDown();
                        break;
                    case 0:
                        break;
                }
            }
        }
        
        public override void Quit()
        {
            RoundManager.ApplySettingInStateByPausing(false);
            _menuManager.OnDisableMenu();
            PlayersControlManager.AcceptAllPlayersInput();
        }
    }
}