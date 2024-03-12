namespace GameState
{
    public class GameStartingState : GameStateInterface
    {
        public GameStartingState(GameRoundStateManager manager) 
            : base(manager, GameRoundManager.GameState.Start) { }

        public override void Enter()
        {
            
            RoundManager.ApplySettingInStateByPausing(false);
            RoundManager.BlockAllPlayersInput();
        }
        
        public override void Update()
        {
            if (!RoundManager.CheckAnimationEndedOfAllPlayers()) return;
            
            
            RoundStateManager.ChangeState(GameRoundManager.GameState.NormalPlay);
        }
        
        public override void Quit()
        {
            RoundManager.ResetCharactersAnimationEndedPoint();
            RoundManager.AcceptAllPlayersInput();
        }
    }
}