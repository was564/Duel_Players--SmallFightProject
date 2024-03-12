using System.Collections.Generic;
using Character.PlayerMode;
using UnityEngine;

namespace GameRound.PlayersInitializeInRoundClass
{
    public class StartingRoundIntialize : PlayersInitializeInRoundFactory
    {
        public StartingRoundIntialize(List<PlayerCharacter> players)
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
                player.StateManager.ChangeState(BehaviorEnumSet.State.IntroPose);
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