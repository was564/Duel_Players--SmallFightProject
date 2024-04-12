using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class GrabWaitState : BehaviorStateInterface
    {
        private PlayerCharacter EnemyCharacterScript;
        private Transform EnemyTransform;
        
        private int _waitFrameForEscape = 15;
        private int _startFrameThisState;
        
        private Vector3 _enemyPositionGrabOffSet = new Vector3(0.8f, 0.2f, 0.0f);

        public GrabWaitState(GameObject characterRoot) :
            base(BehaviorEnumSet.State.GrabWait, characterRoot,
                BehaviorEnumSet.AttackLevel.SpecialMove, PassiveStateEnumSet.CharacterPositionState.OnGround)
        {
            EnemyCharacterScript = PlayerCharacter.EnemyObject.GetComponent<PlayerCharacter>();
            EnemyTransform = PlayerCharacter.EnemyObject.transform;
        }
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            PlayerCharacter.LookAtEnemy();
            
            CharacterAnimator.PauseAnimation();
            _startFrameThisState = FrameManager.CurrentFrame;
            
            EnemyCharacterScript.StateManager.ChangeState(BehaviorEnumSet.State.BeCaught);
            
            Vector3 enemyPosition = PlayerCharacter.transform.position;
            if (PlayerCharacter.transform.forward.x > 0)
                enemyPosition.x += _enemyPositionGrabOffSet.x;
            else enemyPosition.x -= _enemyPositionGrabOffSet.x;
            EnemyTransform.position = enemyPosition;
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
            if(EnemyCharacterScript.StateManager.CurrentState.StateName == BehaviorEnumSet.State.GrabEscape)
            {
                return BehaviorEnumSet.State.GrabEscape;
            }
            
            if(FrameManager.CurrentFrame - _startFrameThisState > _waitFrameForEscape)
            {
                return BehaviorEnumSet.State.GrabAttack;
            }
            
            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            CharacterAnimator.ResumeAnimation();
        }
    }
}