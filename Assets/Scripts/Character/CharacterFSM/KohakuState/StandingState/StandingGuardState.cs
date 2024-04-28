using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class StandingGuardState: GuardState
    {
        public StandingGuardState(GameObject characterRoot, GameObject wall) : 
            base(characterRoot, wall, BehaviorEnumSet.State.StandingGuard, BehaviorEnumSet.State.StandingIdle, 
                PassiveStateEnumSet.CharacterPositionState.OnGround) {}
        
        public override void Enter()
        {
            base.Enter();
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            CharacterAnimator.PlayAnimation("StandingStop", CharacterAnimator.Layer.LowerLayer, true);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            PressGuardKey(behavior);

            if (ContinuousFrameByBlockAttack > 0) return BehaviorEnumSet.State.Null;
            
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Jump:
                    CharacterRigidBody.velocity = (CharacterTransform.forward.x > 0 ? -1.0f : 1.0f) * 2.5f * Vector3.right;
                    return BehaviorEnumSet.State.Jump;
                
                case BehaviorEnumSet.Behavior.Crouch:
                    return BehaviorEnumSet.State.CrouchGuard;
                
                case BehaviorEnumSet.Behavior.BackStep:
                    return BehaviorEnumSet.State.BackStepOnGroundState;
                
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