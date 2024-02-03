using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    public class GameStateManager
    {
        private Dictionary<GameRoundManager.GameState, GameStateInterface> _states;
        
        private GameStateInterface _currentState;
        
        // implement the RoundManager later to avoid inclusion in circular reference
        public GameRoundManager RoundManager { get; set; }
        
        public GameStateManager()
        {
            InitStates();
            RoundManager = GameObject.FindObjectOfType<GameRoundManager>();
            _currentState = _states[GameRoundManager.GameState.NormalPlay];
        }
        
        public void Update()
        {
            _currentState.Update();
        }

        public void ChangeState(GameRoundManager.GameState state)
        {
            _currentState = _states[state];
        }

        private void InitStates()
        {
            _states = new Dictionary<GameRoundManager.GameState, GameStateInterface>();
            _states.Add(GameRoundManager.GameState.NormalPlay, new GameNormalPlayingState(this));
            _states.Add(GameRoundManager.GameState.Start, new GameStartingState(this));
            _states.Add(GameRoundManager.GameState.Replay, new GameReplayingState(this));
            _states.Add(GameRoundManager.GameState.Pause, new GamePausingState(this));
            _states.Add(GameRoundManager.GameState.End, new GameEndingState(this));
        }
    }
}