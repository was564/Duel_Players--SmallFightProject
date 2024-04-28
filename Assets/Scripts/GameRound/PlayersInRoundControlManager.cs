using System.Collections.Generic;
using Character.PlayerMode;
using GameRound.PlayersInitializeInRoundClass;
using UnityEngine;

namespace GameRound
{
    public class PlayersInRoundControlManager
    {
        

        private Dictionary<GameRoundManager.GameState, PlayersInitializeInRoundFactory> _initializationRoundStates;

        private Dictionary<PlayerCharacter.CharacterIndex, PlayerCharacter> _players;
        // private List<PlayerCharacter> _players = new List<PlayerCharacter>();
        // private List<PlayerModeManager.PlayerMode> _previousStates = new List<PlayerModeManager.PlayerMode>();
        private Dictionary<PlayerCharacter.CharacterIndex, PlayerModeManager.PlayerMode> _previousStates;
        private CharacterInputManager[] _inputManagers;
        
        //private PlayersInitializeInRoundFactory _playersInitializationManager;
        
        public PlayersInRoundControlManager()
        {
            PlayerCharacter player = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<PlayerCharacter>();
            PlayerCharacter enemy = GameObject.FindGameObjectWithTag("Enemy").transform.root.GetComponent<PlayerCharacter>();
            _inputManagers = GameObject.FindObjectsOfType<CharacterInputManager>();

            _players = new Dictionary<PlayerCharacter.CharacterIndex, PlayerCharacter>(2);
            _previousStates = new Dictionary<PlayerCharacter.CharacterIndex, PlayerModeManager.PlayerMode>(2)
            {
                {PlayerCharacter.CharacterIndex.Player, PlayerModeManager.PlayerMode.GamePause},
                {PlayerCharacter.CharacterIndex.Enemy, PlayerModeManager.PlayerMode.GamePause}
            };
            
            _players.Add(PlayerCharacter.CharacterIndex.Player, player);
            player.PlayerUniqueIndex = PlayerCharacter.CharacterIndex.Player;
       
            _players.Add(PlayerCharacter.CharacterIndex.Enemy, enemy);
            enemy.PlayerUniqueIndex = PlayerCharacter.CharacterIndex.Enemy;

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
            foreach (var player in _players)
            {
                _previousStates[player.Key] = _players[player.Key].GetPlayerMode();
                _players[player.Key].SetPlayerMode(mode);
            }
        }

        public void ChangeModeOfPlayer(PlayerCharacter.CharacterIndex index, PlayerModeManager.PlayerMode mode)
        {
            _previousStates[index] = _players[index].GetPlayerMode();
            _players[index].SetPlayerMode(mode);
        }

        public void ChangeModeToStopOfAllPlayers()
        {
            foreach (var player in _players)
            {
                _previousStates[player.Key] = player.Value.GetPlayerMode();
                player.Value.SetPlayerMode(PlayerModeManager.PlayerMode.GamePause);
            }
        }
        
        public void ChangePreviousModeOfAllPlayers()
        {
            foreach (var player in _players)
            {
                player.Value.SetPlayerMode(_previousStates[player.Key]);
            }
        }
        
        public void ResetPlayersAnimationEndedPoint()
        {
            foreach (var player in _players)
            {
                player.Value.IsEndedPoseAnimation = false;
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

        public void AssignReplayingInputQueueToPlayer(PlayerCharacter.CharacterIndex characterIndex, Queue<EntryState> inputQueue)
        {
            _players[characterIndex].SetReplayingInputQueue(inputQueue);
        }
        
        public bool GetIsInitializedPlayers()
        {
            foreach (var player in _players)
            {
                if (!player.Value.IsInitializedStartMethod)
                    return false;
            }

            return true;
        }
        
        public void InitializePlayersInStartingGame()
        {
            foreach (var player in _players)
            {
                player.Value.Initialize();
            }
        }
        
        public float GetDistanceBetweenPlayers()
        {
            return Mathf.Abs(
                _players[PlayerCharacter.CharacterIndex.Player].transform.position.x - _players[PlayerCharacter.CharacterIndex.Enemy].transform.position.x);
        }
        
        public int CountAnimationEndedOfAllPlayers()
        {
            int count = 0;
            foreach (var player in _players)
            {
                if (player.Value.IsEndedPoseAnimation) count++;
            }

            return count;
        }
        
        public int CountDownPlayers()
        {
            int count = 0;
            foreach (var player in _players) 
                if (player.Value.Hp <= 0) count++;
            
            return count;
        }
        
        // this method can get only one down player's index
        // please Check CountDownPlayers() result is 1 before calling this method
        public PlayerCharacter.CharacterIndex GetDownPlayerIndex()
        {
            if (_players[PlayerCharacter.CharacterIndex.Player].Hp <= 0) return PlayerCharacter.CharacterIndex.Player;
            if (_players[PlayerCharacter.CharacterIndex.Enemy].Hp <= 0) return PlayerCharacter.CharacterIndex.Enemy;
            return PlayerCharacter.CharacterIndex.Size;
        }

        public PlayerCharacter.CharacterIndex GetLowestHpCharacterIndex()
        {
            int playerHp = _players[PlayerCharacter.CharacterIndex.Player].Hp;
            int enemyHp = _players[PlayerCharacter.CharacterIndex.Enemy].Hp;
            
            if (playerHp > enemyHp) return PlayerCharacter.CharacterIndex.Enemy;
            else if (playerHp == enemyHp) return PlayerCharacter.CharacterIndex.Size;
            else return PlayerCharacter.CharacterIndex.Player;
        }
    }
}