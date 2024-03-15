namespace GameRound
{
    public class GameRoundWaitingUntilEndState : GameRoundStateInterface
    {
        public GameRoundWaitingUntilEndState(GameRoundStateManager manager)
            : base(manager, GameRoundManager.GameState.Wait) { }

        public override void Enter()
        {
            RoundStateManager.FrameManager.IsFramePaused = true;
            PlayersControlManager.BlockAllPlayersInput();
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