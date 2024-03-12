using System.Collections.Generic;
using Character.PlayerMode;
using UnityEngine;

namespace GameRound
{
    public class CharactersInRoundControlManager
    {
        public enum CharacterIndex
        {
            Player = 0,
            Enemy,
            Size
        }
        
        private List<PlayerCharacter> _players = new List<PlayerCharacter>();
        private CharacterInputManager[] _inputManagers;

        public CharactersInRoundControlManager()
        {
            PlayerCharacter player = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<PlayerCharacter>();
            PlayerCharacter enemy = GameObject.FindGameObjectWithTag("Enemy").transform.root.GetComponent<PlayerCharacter>();
            _inputManagers = GameObject.FindObjectsOfType<CharacterInputManager>();

            _players.Insert((int)CharacterIndex.Player, player);
            player.PlayerUniqueIndex = _players.Count;
       
            _players.Insert((int)CharacterIndex.Enemy, enemy);
            enemy.PlayerUniqueIndex = _players.Count;
        }

        public void 
        
        public void PauseAllPlayers()
        {
            foreach (var player in _players)
            {
                player.SetPlayerMode(PlayerModeManager.PlayerMode.GamePause);
            }
        }
    }
}