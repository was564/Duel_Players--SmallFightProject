using System.Collections.Generic;
using UnityEngine;
using Character.PlayerMode;

namespace GameRound.PlayersInitializeInRoundClass
{
    public class SingleNormalPlayingRoundInitialize : PlayersInitializeInRoundFactory
    {
        public SingleNormalPlayingRoundInitialize(Dictionary<PlayerCharacter.CharacterIndex, PlayerCharacter> players)
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
            
            Players[PlayerCharacter.CharacterIndex.Player].SetPlayerMode(PlayerModeManager.PlayerMode.NormalPlaying);
            Players[PlayerCharacter.CharacterIndex.Enemy].SetPlayerMode(PlayerModeManager.PlayerMode.AI);

            Players[PlayerCharacter.CharacterIndex.Player].IsAcceptArtificialInput = false;
            Players[PlayerCharacter.CharacterIndex.Enemy].IsAcceptArtificialInput = false;
            
        }
    }
}