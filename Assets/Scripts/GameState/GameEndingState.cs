namespace GameState
{
    public class GameEndingState : GameStateInterface
    {
        public GameEndingState(GameStateManager manager) 
            : base(manager, GameRoundManager.GameState.End) { }

        public override void Enter()
        {
            
        }
        
        public override void Update()
        {
            throw new System.NotImplementedException();
        }
        
        public override void Quit()
        {
            throw new System.NotImplementedException();
        }
    }
}