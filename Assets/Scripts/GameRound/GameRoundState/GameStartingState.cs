namespace GameRound
{
    public class GameStartingState : GameStateInterface
    {
        public GameStartingState(GameRoundStateManager manager) 
            : base(manager, GameRoundManager.GameState.Start) { }

        public override void Enter()
        {
            RoundManager.ApplySettingInStateByPausing(false);
            PlayersControlManager.BlockAllPlayersInput();
        }
        
        public override void Update()
        {
            if (PlayersControlManager.CountAnimationEndedOfAllPlayers() < 2) return;
            
            
            RoundStateManager.ChangeState(GameRoundManager.GameState.NormalPlay);
        }
        
        public override void Quit()
        {
            PlayersControlManager.ResetPlayersAnimationEndedPoint();
            PlayersControlManager.AcceptAllPlayersInput();
        }
    }
}