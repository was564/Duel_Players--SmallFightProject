using System.Collections.Generic;
using Character.PlayerMode;
using UnityEngine;

namespace GameRound.PlayersInitializeInRoundClass
{
    public class ReplayingRoundInitialize : PlayersInitializeInRoundFactory
    {
        public ReplayingRoundInitialize(Dictionary<PlayerCharacter.CharacterIndex, PlayerCharacter> players)
            : base(players) {}
        
        protected override void InitializePlayersPosition()
        {
            Players[PlayerCharacter.CharacterIndex.Player].transform.position = Vector3.left;
            Players[PlayerCharacter.CharacterIndex.Enemy].transform.position = Vector3.right;
        }

        protected override void InitializePlayersInitState()
        {
            foreach (var player in Players)
            {
                player.Value.StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
            }
        }

        protected override void InitializePlayersMode()
        {
            foreach (var player in Players)
            {
                player.Value.SetPlayerMode(PlayerModeManager.PlayerMode.Replaying);
                player.Value.IsAcceptArtificialInput = false;
            }
        }
    }
}