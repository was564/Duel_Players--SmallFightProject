using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public abstract class HitState : BehaviorStateInterface
    {
        public HitState(GameObject characterRoot, BehaviorEnumSet.State hitStateName, PassiveStateEnumSet.CharacterPositionState position) :
            base(hitStateName, characterRoot, BehaviorEnumSet.AttackLevel.Hit, position)
        {}
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            return BehaviorEnumSet.State.Null;
        }
        
        public override void Quit()
        {
            
        }
    }
}