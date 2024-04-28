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

        public static bool IsHitState(BehaviorEnumSet.State state)
        {
            return (state == BehaviorEnumSet.State.StandingHit ||
                    state == BehaviorEnumSet.State.CrouchHit ||
                    state == BehaviorEnumSet.State.InAirHit);
        }
        
        public static bool IsGuardedState(BehaviorEnumSet.State state)
        {
            return (state == BehaviorEnumSet.State.CrouchGuard) ||
                   (state == BehaviorEnumSet.State.StandingGuard);
        }
        
    }
}