using UnityEngine;

namespace GameRound
{
    public class GameRoundResultState : GameRoundStateInterface
    {
        private bool _isDraw;
        private MenuInGame _menuManager;

        public GameRoundResultState(GameRoundStateManager manager)
            : base(manager, GameRoundManager.GameState.Result)
        {
            _menuManager = GameObject.FindObjectOfType<MenuInGame>();
        }

        public override void Enter()
        {
            RoundStateManager.FrameManager.IsFramePaused = true;
            RoundManager.ApplySettingInStateByPausing(false);
            PlayersControlManager.InitializePlayersInRound(StateName);
            
            if (RoundManager.IsGameEnded) return;
            switch (PlayersControlManager.CountDownPlayers())
            {
                case 0:
                    _isDraw = true;
                    break;
                case 1:
                    _isDraw = false;
                    break;
                case 2:
                    _isDraw = true;
                    break;
            }

            if (_isDraw) RoundManager.DrawRound();
            else RoundManager.EndRound(
                (PlayersInRoundControlManager.CharacterIndex)((short)PlayersControlManager.GetDownPlayerIndex() ^ 1));
        }
        
        public override void Update()
        {
            
            

            if (InputManager.IsPressedMenuEnterKey())
            {
                _menuManager.SelectMenuOption();
            }
        }
        
        public override void Quit()
        {
            
        }
    }
}