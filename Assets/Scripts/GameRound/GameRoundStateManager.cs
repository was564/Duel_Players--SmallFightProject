using System.Collections.Generic;
using UnityEngine;

namespace GameRound
{
    public class GameRoundStateManager
    {
        private Dictionary<GameRoundManager.GameState, GameRoundStateInterface> _states;
        
        private GameRoundStateInterface _currentRoundState;
        public GameRoundManager.GameState PreviousStateName { get; private set; }
        
        public GameRoundManager RoundManager { get; private set; }
        
        public PlayersInRoundControlManager PlayersControlManager { get; private set; }
        
        public GameRoundStateManager(GameRoundManager roundManager, PlayersInRoundControlManager controlManager, FrameManager frameManager)
        {
            RoundManager = roundManager;
            PlayersControlManager = controlManager;
            InitStates();
            _currentRoundState = _states[GameRoundManager.GameState.Start];
        }
        
        public void Update()
        {
            _currentRoundState.Update();
        }

        public void ChangeState(GameRoundManager.GameState state)
        {
            _currentRoundState.Quit();
            PreviousStateName = _currentRoundState.StateName;
            _currentRoundState = _states[state];
            _currentRoundState.Enter();
        }

        private void InitStates()
        {
            _states = new Dictionary<GameRoundManager.GameState, GameRoundStateInterface>();
            _states.Add(GameRoundManager.GameState.NormalPlay, new GameRoundNormalPlayingState(this));
            _states.Add(GameRoundManager.GameState.Start, new GameRoundStartingState(this));
            _states.Add(GameRoundManager.GameState.Replay, new GameRoundReplayingState(this));
            _states.Add(GameRoundManager.GameState.Pause, new GameRoundPausingState(this));
            _states.Add(GameRoundManager.GameState.End, new GameRoundEndingState(this));
            _states.Add(GameRoundManager.GameState.Wait, new GameRoundWaitingUntilEndState(this));
            _states.Add(GameRoundManager.GameState.Result, new GameRoundResultState(this));
        }
    }
}