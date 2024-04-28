namespace GameRound
{
    public class GameRoundReplayingState : GameRoundStateInterface
    {
        public GameRoundReplayingState(GameRoundStateManager manager) 
            : base(manager, GameRoundManager.GameState.Replay) { }

        public override void Enter()
        {
            FrameManager.IsFramePaused = false;
            RoundManager.ApplySettingInStateByPausing(false);
        }

        public override void Update()
        {
            if (RoundManager.IsGameEnded)
                RoundStateManager.ChangeState(GameRoundManager.GameState.Wait);
            
            if(InputManager.IsPressedMenuKey())
                RoundStateManager.ChangeState(GameRoundManager.GameState.Pause);
            
            RoundManager.DecreaseRemainTimePerFrame(1);
        }
        
        public override void Quit()
        {
            FrameManager.IsFramePaused = true;
        }
    }
}