using System.Collections.Generic;
using Character.PlayerMode;
using UnityEngine;

namespace GameRound.PlayersInitializeInRoundClass
{
    public class ReplayingRoundInitialize : PlayersInitializeInRoundFactory
    {
        public ReplayingRoundInitialize(List<PlayerCharacter> players)
            : base(players) {}
        
        protected override void InitializePlayersPosition()
        {
            Players[(int)PlayersInRoundControlManager.CharacterIndex.Player].transform.position = Vector3.left;
            Players[(int)PlayersInRoundControlManager.CharacterIndex.Enemy].transform.position = Vector3.right;
        }

        protected override void InitializePlayersInitState()
        {
            foreach (var player in Players)
            {
                player.StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
            }
        }

        protected override void InitializePlayersMode()
        {
            foreach (var player in Players)
            {
                player.SetPlayerMode(PlayerModeManager.PlayerMode.Replaying);
                player.IsAcceptArtificialInput = false;
            }
        }
    }
}