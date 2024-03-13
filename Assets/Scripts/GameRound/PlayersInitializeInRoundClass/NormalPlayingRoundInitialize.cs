using System.Collections.Generic;
using UnityEngine;
using Character.PlayerMode;

namespace GameRound.PlayersInitializeInRoundClass
{
    public class NormalPlayingRoundInitialize : PlayersInitializeInRoundFactory
    {
        public NormalPlayingRoundInitialize(List<PlayerCharacter> players)
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
                player.SetPlayerMode(PlayerModeManager.PlayerMode.NormalPlaying);
                player.IsAcceptArtificialInput = false;
            }
        }
    }
}