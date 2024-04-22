using System.Collections.Generic;

namespace Character.CharacterFSM
{
    public class PlayerStateCheckingMethodSet
    {
        
        public static bool CheckState(BehaviorEnumSet.State currentState, BehaviorEnumSet.State targetState)
        {
            return (currentState == targetState);
        }
        
        public static bool IsAttackState(BehaviorEnumSet.State state)
        {
            return ((int)state > (int)BehaviorEnumSet.State.AttackStartIndex &&
                    (int)state < (int)BehaviorEnumSet.State.AttackEndIndex);
        }

        public static bool IsHittedState(BehaviorEnumSet.State state)
        {
            return (state == BehaviorEnumSet.State.StandingHit ||
                    state == BehaviorEnumSet.State.CrouchHit ||
                    state == BehaviorEnumSet.State.InAirHit);
        }
        
    }
}