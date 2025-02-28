using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class GrabStartState : BehaviorStateInterface
    {
        
        private PlayerCharacter EnemyCharacterScript;

        public GrabStartState(GameObject characterRoot) :
            base(BehaviorEnumSet.State.GrabStart, characterRoot,
                BehaviorEnumSet.AttackLevel.BasicAttack, PassiveStateEnumSet.CharacterPositionState.OnGround)
        {
            EnemyCharacterScript = PlayerCharacter.EnemyObject.GetComponent<PlayerCharacter>();
        }
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            PlayerCharacter.LookAtEnemy();
            
            CharacterAnimator.PlayAnimation("Grab", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimation("StandingIdle", CharacterAnimator.Layer.LowerLayer);
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
            if(CharacterAnimator.IsEndCurrentAnimation("Grab", CharacterAnimator.Layer.UpperLayer))
                return BehaviorEnumSet.State.StandingIdle;
            
            if(CharacterAnimator.GetCurrentAnimationDuration(CharacterAnimator.Layer.UpperLayer) >= 0.4f
               && CharacterAnimator.GetCurrentAnimationDuration(CharacterAnimator.Layer.UpperLayer) <= 0.5f
               && EnemyCharacterScript.CurrentCharacterPositionState != PassiveStateEnumSet.CharacterPositionState.InAir
               && EnemyCharacterScript.StateManager.CurrentState.AttackLevel < (int)BehaviorEnumSet.AttackLevel.Guard
               && Mathf.Abs(PlayerCharacter.EnemyObject.transform.position.x - PlayerCharacter.transform.position.x) < 1.0f)
            {
                CharacterJudgeBoxController.GetAttackBox(BehaviorEnumSet.State.GrabAttack).PlayHitEffect();
                return BehaviorEnumSet.State.GrabWait;
            }
            
            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            
        }
    }
}