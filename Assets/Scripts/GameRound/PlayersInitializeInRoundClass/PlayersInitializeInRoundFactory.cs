using System.Collections.Generic;
using UnityEngine;

namespace GameRound
{
    public abstract class PlayersInitializeInRoundFactory
    {
        protected Dictionary<PlayerCharacter.CharacterIndex, PlayerCharacter> Players;
        
        public PlayersInitializeInRoundFactory(Dictionary<PlayerCharacter.CharacterIndex, PlayerCharacter> players)
        {
            Players = players;
        }
        
        public void InitializePlayersInRound()
        {
            InitializePlayersPosition();
            
            foreach (var player in Players)
            {
                player.Value.ComboManagerInstance.IsCanceled = true;
                player.Value.IsGuarded = false;
                
                player.Value.GetComponent<CharacterJudgeBoxController>().EnableHitBox();
                player.Value.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            InitializePlayersMode();
            InitializePlayersInitState();
        }

        protected abstract void InitializePlayersPosition();
        
        protected abstract void InitializePlayersInitState();

        protected abstract void InitializePlayersMode();
        
    }
}