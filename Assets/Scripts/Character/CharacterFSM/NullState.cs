using UnityEngine;

namespace Character.CharacterFSM
{
    public class NullState : BehaviorStateInterface
    {
        public NullState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.Null, characterRoot, 
                BehaviorEnumSet.AttackLevel.Size, PassiveStateEnumSet.CharacterPositionState.Size) {}

        public override void Enter()
        {
            return;
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            return BehaviorEnumSet.State.Null;
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            return;
        }
    }
}