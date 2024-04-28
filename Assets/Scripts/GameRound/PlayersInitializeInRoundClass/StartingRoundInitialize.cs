using System.Collections.Generic;
using Character.PlayerMode;
using UnityEngine;

namespace GameRound.PlayersInitializeInRoundClass
{
    public class StartingRoundInitialize : PlayersInitializeInRoundFactory
    {
        public StartingRoundInitialize(Dictionary<PlayerCharacter.CharacterIndex, PlayerCharacter> players)
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
                player.Value.ResetHp();
                player.Value.StateManager.ChangeState(BehaviorEnumSet.State.IntroPose);
            }

            foreach (var player in Players)
            {
                player.Value.GetComponent<CharacterAnimator>().ResumeAnimation();
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