namespace GameState
{
    public abstract class GameStateInterface
    {

        public GameStateInterface(GameStateManager stateManager, GameRoundManager.GameState stateName)
        {
            StateName = stateName;
            StateManager = stateManager;
            RoundManager = stateManager.RoundManager;
        }

        public GameRoundManager.GameState StateName { get; private set; }
        
        protected GameStateManager StateManager { get; private set; }
        
        protected GameRoundManager RoundManager { get; private set; }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Quit();
    }
}