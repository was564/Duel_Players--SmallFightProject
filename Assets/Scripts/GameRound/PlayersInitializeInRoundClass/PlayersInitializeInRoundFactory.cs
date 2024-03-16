using System.Collections.Generic;
using UnityEngine;

namespace GameRound
{
    public abstract class PlayersInitializeInRoundFactory
    {
        protected List<PlayerCharacter> Players;
        
        public PlayersInitializeInRoundFactory(List<PlayerCharacter> players)
        {
            Players = players;
        }
        
        public void InitializePlayersInRound()
        {
            InitializePlayersPosition();
            
            foreach (var player in Players)
            {
                player.ComboManagerInstance.IsCanceled = true;
                player.IsGuarded = false;
                
                player.GetComponent<CharacterJudgeBoxController>().EnableHitBox();
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            InitializePlayersMode();
            InitializePlayersInitState();
        }

        protected abstract void InitializePlayersPosition();
        
        protected abstract void InitializePlayersInitState();

        protected abstract void InitializePlayersMode();
        
    }
}