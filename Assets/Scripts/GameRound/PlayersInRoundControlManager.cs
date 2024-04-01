using System.Collections.Generic;
using Character.PlayerMode;
using GameRound.PlayersInitializeInRoundClass;
using UnityEngine;

namespace GameRound
{
    public class PlayersInRoundControlManager
    {
        public enum CharacterIndex
        {
            Player = 0,
            Enemy,
            Size
        }

        private Dictionary<GameRoundManager.GameState, PlayersInitializeInRoundFactory> _initializationRoundStates;
        
        private List<PlayerCharacter> _players = new List<PlayerCharacter>();
        private List<PlayerModeManager.PlayerMode> _previousStates = new List<PlayerModeManager.PlayerMode>();
        private CharacterInputManager[] _inputManagers;
        
        //private PlayersInitializeInRoundFactory _playersInitializationManager;
        
        public PlayersInRoundControlManager()
        {
            PlayerCharacter player = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<PlayerCharacter>();
            PlayerCharacter enemy = GameObject.FindGameObjectWithTag("Enemy").transform.root.GetComponent<PlayerCharacter>();
            _inputManagers = GameObject.FindObjectsOfType<CharacterInputManager>();

            _previousStates = new List<PlayerModeManager.PlayerMode>(2) 
                { PlayerModeManager.PlayerMode.GamePause, PlayerModeManager.PlayerMode.GamePause };
            
            _players.Insert((int)CharacterIndex.Player, player);
            player.PlayerUniqueIndex = _players.Count;
       
            _players.Insert((int)CharacterIndex.Enemy, enemy);
            enemy.PlayerUniqueIndex = _players.Count;

            _initializationRoundStates = new Dictionary<GameRoundManager.GameState, PlayersInitializeInRoundFactory>();
            _initializationRoundStates.Add(GameRoundManager.GameState.Start, new StartingRoundInitialize(_players));
            _initializationRoundStates.Add(GameRoundManager.GameState.NormalPlay, new NormalPlayingRoundInitialize(_players));
            _initializationRoundStates.Add(GameRoundManager.GameState.Replay, new ReplayingRoundInitialize(_players));
            _initializationRoundStates.Add(GameRoundManager.GameState.End, new EndingRoundInitialize(_players));
            
            //_playersInitializationManager = _initializationRoundStates[GameRoundManager.GameState.NormalPlay];
        }

        public void InitializePlayersInRound(GameRoundManager.GameState state)
        {
            if (!_initializationRoundStates.ContainsKey(state)) return;
            
            _initializationRoundStates[state].InitializePlayersInRound();
            //_playersInitializationManager.InitializePlayersInRound();
        }
        
        /*
        public void ChangeInitializeRoundState(GameRoundManager.GameState state)
        {
            _playersInitializationManager = _initializationRoundStates[state];
        }
        */
        
        public void ChangeModeOfAllPlayers(PlayerModeManager.PlayerMode mode)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                _previousStates[i] = _players[i].GetPlayerMode();
                _players[i].SetPlayerMode(mode);
            }
        }

        public void ChangeModeOfPlayer(CharacterIndex index, PlayerModeManager.PlayerMode mode)
        {
            _previousStates[(int)index] = _players[(int)index].GetPlayerMode();
            _players[(int)index].SetPlayerMode(mode);
        }

        public void ChangeModeToStopOfAllPlayers()
        {
            for (int i=0;i<_players.Count;i++)
            {
                _previousStates[i] = _players[i].GetPlayerMode();
                _players[i].SetPlayerMode(PlayerModeManager.PlayerMode.GamePause);
            }
        }
        
        public void ChangePreviousModeOfAllPlayers()
        {
            for (int i=0;i<_players.Count;i++)
            {
                _players[i].SetPlayerMode(_previousStates[i]);
            }
        }
        
        public void ResetPlayersAnimationEndedPoint()
        {
            foreach (var player in _players)
            {
                player.IsEndedPoseAnimation = false;
            }
        }
        
        public void BlockAllPlayersInput()
        {
            foreach (var manager in _inputManagers)
            {
                manager.IsAvailableInput = false;
            }
        }
        
        public void AcceptAllPlayersInput()
        {
            foreach (var manager in _inputManagers)
            {
                manager.IsAvailableInput = true;
            }
        }
        
        public float GetDistanceBetweenPlayers()
        {
            return Mathf.Abs(_players[(int)CharacterIndex.Player].transform.position.x - _players[(int)CharacterIndex.Enemy].transform.position.x);
        }
        
        public int CountAnimationEndedOfAllPlayers()
        {
            int count = 0;
            foreach (var player in _players)
            {
                if (player.IsEndedPoseAnimation) count++;
            }

            return count;
        }
        
        public int CountDownPlayers()
        {
            int count = 0;
            foreach (var player in _players) 
                if (player.Hp <= 0) count++;
            
            return count;
        }
        
        // this method can get only one down player's index
        // please Check CountDownPlayers() result is 1 before calling this method
        public CharacterIndex GetDownPlayerIndex()
        {
            if (_players[(int)CharacterIndex.Player].Hp <= 0) return CharacterIndex.Player;
            if (_players[(int)CharacterIndex.Enemy].Hp <= 0) return CharacterIndex.Enemy;
            return CharacterIndex.Size;
        }

        public CharacterIndex GetLowestHpCharacterIndex()
        {
            int playerHp = _players[(int)CharacterIndex.Player].Hp;
            int enemyHp = _players[(int)CharacterIndex.Enemy].Hp;
            
            if (playerHp > enemyHp) return CharacterIndex.Enemy;
            else if (playerHp == enemyHp) return CharacterIndex.Size;
            else return CharacterIndex.Player;
        }
    }
}