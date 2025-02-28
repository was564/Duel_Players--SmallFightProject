using UnityEngine;

namespace GameRound
{
    public class GameRoundPausingState : GameRoundStateInterface
    {
        private MenuInGame _menuManager;
        private SoundManager _soundManager;

        public GameRoundPausingState(GameRoundStateManager manager)
            : base(manager, GameRoundManager.GameState.Pause)
        {
            _menuManager = GameObject.FindObjectOfType<MenuInGame>();
            _soundManager = GameObject.FindObjectOfType<SoundManager>();
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
                RoundStateManager.ChangeState(RoundStateManager.PreviousStateName);
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
                    // up key
                    case 1:
                        _menuManager.MenuScrollUp();
                        _soundManager.PlayEffect(SoundManager.SoundSet.MenuScrollSound);
                        break;
                    // down key
                    case -1:
                        _menuManager.MenuScrollDown();
                        _soundManager.PlayEffect(SoundManager.SoundSet.MenuScrollSound);
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