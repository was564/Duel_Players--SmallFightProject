namespace GameRound
{
    public class GameRoundStartingState : GameRoundStateInterface
    {
        public GameRoundStartingState(GameRoundStateManager manager) 
            : base(manager, GameRoundManager.GameState.Start) { }
        
        public override void Enter()
        {
            RoundManager.ApplySettingInStateByPausing(false);
            PlayersControlManager.BlockAllPlayersInput();
            PlayersControlManager.InitializePlayersInRound(StateName);
        }
        
        public override void Update()
        {
            if (PlayersControlManager.CountAnimationEndedOfAllPlayers() < 2) return;
            
            
            PlayersControlManager.InitializePlayersInRound(RoundManager.GetSelectedGameState());
            RoundStateManager.ChangeState(RoundManager.GetSelectedGameState());
            //PlayersControlManager.ChangeModeToStopOfAllPlayers();
            //PlayersControlManager.ChangePreviousModeOfAllPlayers();
        }
        
        public override void Quit()
        {
            PlayersControlManager.ResetPlayersAnimationEndedPoint();
            PlayersControlManager.AcceptAllPlayersInput();
        }
    }
}