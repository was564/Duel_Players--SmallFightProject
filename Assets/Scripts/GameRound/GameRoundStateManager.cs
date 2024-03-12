using System.Collections.Generic;
using UnityEngine;

namespace GameRound
{
    public class GameRoundStateManager
    {
        private Dictionary<GameRoundManager.GameState, GameStateInterface> _states;
        
        private GameStateInterface _currentState;
        
        public GameRoundManager RoundManager { get; private set; }
        
        public FrameManager FrameManager { get; private set; }
        
        public GameRoundStateManager(GameRoundManager roundManager, FrameManager frameManager)
        {
            RoundManager = roundManager;
            FrameManager = frameManager;
            InitStates();
            _currentState = _states[GameRoundManager.GameState.Start];
        }
        
        public void Update()
        {
            _currentState.Update();
        }

        public void ChangeState(GameRoundManager.GameState state)
        {
            _currentState.Quit();
            _currentState = _states[state];
            _currentState.Enter();
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