namespace GameRound
{
    public class GameReplayingState : GameStateInterface
    {
        public GameReplayingState(GameRoundStateManager manager) 
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