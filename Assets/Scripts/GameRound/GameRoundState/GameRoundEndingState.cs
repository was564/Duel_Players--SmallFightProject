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
            _isEndedWaiting = false;
            _waitFrame = 0;
            RoundManager.ApplySettingInStateByPausing(false);
        }
        
        public override void Update()
        {
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

            if (PlayersControlManager.CountAnimationEndedOfAllPlayers() >= 1)
            {
                
            }
            
            throw new System.NotImplementedException();
        }
        
        public override void Quit()
        {
            throw new System.NotImplementedException();
        }
    }
}