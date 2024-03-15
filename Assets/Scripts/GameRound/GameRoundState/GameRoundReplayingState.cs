namespace GameRound
{
    public class GameRoundReplayingState : GameRoundStateInterface
    {
        public GameRoundReplayingState(GameRoundStateManager manager) 
            : base(manager, GameRoundManager.GameState.Replay) { }

        public override void Enter()
        {
            RoundManager.ApplySettingInStateByPausing(false);
        }

        public override void Update()
        {
            
        }
        
        public override void Quit()
        {
            
        }
    }
}