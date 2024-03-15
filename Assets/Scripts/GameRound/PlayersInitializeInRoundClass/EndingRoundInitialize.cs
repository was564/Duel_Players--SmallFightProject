using System.Collections.Generic;
using Character.PlayerMode;

namespace GameRound.PlayersInitializeInRoundClass
{
    public class EndingRoundInitialize : PlayersInitializeInRoundFactory
    {
        public EndingRoundInitialize(List<PlayerCharacter> players)
            : base(players) {}
        
        protected override void InitializePlayersPosition()
        {
            
        }

        protected override void InitializePlayersInitState()
        {
            foreach (var player in Players)
            {
                
                player.StateManager.ChangeState(BehaviorEnumSet.State.OutroPose);
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