namespace GameState
{
    public class GamePausingState : GameStateInterface
    {
        public GamePausingState(GameStateManager manager) 
            : base(manager, GameRoundManager.GameState.Pause) { }

        public override void Enter()
        {
            
        }
        
        public override void Update()
        {
            
        }
        
        public override void Quit()
        {
            
        }
    }
}