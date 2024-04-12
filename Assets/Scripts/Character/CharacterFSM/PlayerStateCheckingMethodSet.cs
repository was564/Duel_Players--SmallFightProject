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

    }
}