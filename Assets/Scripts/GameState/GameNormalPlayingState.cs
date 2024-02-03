namespace GameState
{
    public class GameNormalPlayingState : GameStateInterface
    {
        public GameNormalPlayingState(GameStateManager manager)
            : base(manager, GameRoundManager.GameState.NormalPlay) { }

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