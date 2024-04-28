namespace GameRound
{
    public class GameRoundWaitingUntilEndState : GameRoundStateInterface
    {
        public GameRoundWaitingUntilEndState(GameRoundStateManager manager)
            : base(manager, GameRoundManager.GameState.Wait) { }

        public override void Enter()
        {
            FrameManager.IsFramePaused = true;
            PlayersControlManager.BlockAllPlayersInput();
            RoundManager.ApplySettingInStateByPausing(false);
        }
        
        public override void Update()
        {
            if(PlayersControlManager.CountAnimationEndedOfAllPlayers() >= 1)
                RoundStateManager.ChangeState(GameRoundManager.GameState.End);
        }
        
        public override void Quit()
        {
            
        }
    }
}