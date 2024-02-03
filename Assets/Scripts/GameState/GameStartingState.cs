namespace GameState
{
    public class GameStartingState : GameStateInterface
    {
        public GameStartingState(GameStateManager manager) 
            : base(manager, GameRoundManager.GameState.Start) { }

        public override void Enter()
        {
            
        }
        
        public override void Update()
        {
            StateManager.ChangeState(GameRoundManager.GameState.NormalPlay);
        }
        
        public override void Quit()
        {
            
        }
    }
}