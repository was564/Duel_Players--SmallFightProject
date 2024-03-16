namespace GameRound
{
    public class GameRoundEndingState : GameRoundStateInterface
    {
        private bool _isEndedWaiting = false;
        private int _waitFrame = 0;
        
        public GameRoundEndingState(GameRoundStateManager manager) 
            : base(manager, GameRoundManager.GameState.End) { }

        public override void Enter()
        {
            //_isEndedWaiting = false;
            //_waitFrame = 0;
            RoundStateManager.FrameManager.IsFramePaused = true;
            RoundManager.ApplySettingInStateByPausing(false);
            PlayersControlManager.InitializePlayersInRound(StateName);
        }
        
        public override void Update()
        {
            /*
            if (!_isEndedWaiting)
            {
                _waitFrame++;
                if (_waitFrame >= 60)
                {
                    _waitFrame = 0;
                    _isEndedWaiting = true;
                    
                }
                else return;
            }
            */
            if (PlayersControlManager.CountAnimationEndedOfAllPlayers() >= 2)
                RoundStateManager.ChangeState(GameRoundManager.GameState.Result);
        }
        
        public override void Quit()
        {
            
        }
    }
}