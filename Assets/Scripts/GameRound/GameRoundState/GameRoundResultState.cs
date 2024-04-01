namespace GameRound
{
    public class GameRoundResultState : GameRoundStateInterface
    {
        private bool _isDraw;
        private bool _isShowResults;
        
        public GameRoundResultState(GameRoundStateManager manager) 
            : base(manager, GameRoundManager.GameState.Result) { }

        public override void Enter()
        {
            RoundStateManager.FrameManager.IsFramePaused = true;
            RoundManager.ApplySettingInStateByPausing(false);
            PlayersControlManager.InitializePlayersInRound(StateName);
            
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

            _isShowResults = false;
        }
        
        public override void Update()
        {
            if (_isShowResults)
            {
                _isShowResults = true;
                return;
            }
            
            if (_isDraw) RoundManager.DrawRound();
            else RoundManager.EndRound(
                (PlayersInRoundControlManager.CharacterIndex)((short)PlayersControlManager.GetDownPlayerIndex() ^ 1));

            _isShowResults = true;
        }
        
        public override void Quit()
        {
            
        }
    }
}