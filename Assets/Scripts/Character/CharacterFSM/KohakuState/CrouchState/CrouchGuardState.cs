using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class CrouchGuardState: GuardState
    {
        public CrouchGuardState(GameObject characterRoot, GameObject wall) : 
            base(characterRoot, wall, BehaviorEnumSet.State.CrouchGuard, BehaviorEnumSet.State.CrouchIdle, 
                PassiveStateEnumSet.CharacterPositionState.Crouch) {}
        
        public override void Enter()
        {
            base.Enter();
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            CharacterAnimator.PlayAnimation("CrouchStop", CharacterAnimator.Layer.LowerLayer, true);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            PressGuardKey(behavior);
            
            if(ContinuousFrameByBlockAttack > 0) return BehaviorEnumSet.State.Null;
            
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Stand:
                    return BehaviorEnumSet.State.StandingGuard;
                
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            PlayerCharacter.LookAtEnemy();
            return base.UpdateState();
            
        }

        public override void Quit()
        {
            base.Quit();
        }
    }
}