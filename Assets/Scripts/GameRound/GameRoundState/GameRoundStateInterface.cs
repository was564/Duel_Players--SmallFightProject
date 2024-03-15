using UnityEngine;

namespace GameRound
{
    public abstract class GameRoundStateInterface
    {

        public GameRoundStateInterface(GameRoundStateManager roundStateManager, GameRoundManager.GameState stateName)
        {
            StateName = stateName;
            RoundStateManager = roundStateManager;
            RoundManager = roundStateManager.RoundManager;
            PlayersControlManager = roundStateManager.PlayersControlManager;
            InputManager = GameObject.FindObjectOfType<MenuInputManager>();
        }

        public GameRoundManager.GameState StateName { get; private set; }
        
        protected GameRoundStateManager RoundStateManager { get; private set; }
        
        protected GameRoundManager RoundManager { get; private set; }
        
        protected MenuInputManager InputManager { get; private set; }
        
        protected PlayersInRoundControlManager PlayersControlManager { get; private set; }
        
        public abstract void Enter();
        public abstract void Update();
        public abstract void Quit();
    }
}