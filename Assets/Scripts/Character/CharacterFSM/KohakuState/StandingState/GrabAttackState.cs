using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class GrabAttackState : BehaviorStateInterface
    {
        private PlayerCharacter _enemyCharacterScript;
        
        private bool _isAttacked = false;
        
        private Vector3 _enemyVelocityWhenHit = new Vector3(0.8f, 0.5f, 0f);

        private Vector3 _playerPositionWhenStartingThisState;
        
        private float _positionXAfterAttack = 0.4f;
        
        public GrabAttackState(GameObject characterRoot) : base(BehaviorEnumSet.State.GrabAttack, characterRoot,
            BehaviorEnumSet.AttackLevel.SpecialMove, PassiveStateEnumSet.CharacterPositionState.OnGround)
        {
            _enemyCharacterScript = PlayerCharacter.EnemyObject.GetComponent<PlayerCharacter>();
        }
        
        public override void Enter()
        {
            _isAttacked = false;
            if(PlayerCharacter.transform.forward.x > 0)
            {
                _enemyVelocityWhenHit.x = Mathf.Abs(_enemyVelocityWhenHit.x);
                _positionXAfterAttack = Mathf.Abs(_positionXAfterAttack);
            }
            else
            {
                _enemyVelocityWhenHit.x = -Mathf.Abs(_enemyVelocityWhenHit.x);
                _positionXAfterAttack = -Mathf.Abs(_positionXAfterAttack);
            }

            _playerPositionWhenStartingThisState = CharacterTransform.position;
            
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            CharacterAnimator.PlayAnimation("GrabAttack", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimation("GrabAttack", CharacterAnimator.Layer.LowerLayer);
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
            if(CharacterAnimator.IsEndCurrentAnimation("GrabAttack", CharacterAnimator.Layer.UpperLayer))
            {
                return BehaviorEnumSet.State.StandingIdle;
            }

            if(!_isAttacked
                && CharacterAnimator.GetCurrentAnimationDuration(CharacterAnimator.Layer.UpperLayer) >= 0.5f
               )
            {
                _enemyCharacterScript.StateManager.ChangeState(BehaviorEnumSet.State.InAirHit);
                _enemyCharacterScript.RigidBody.velocity = _enemyVelocityWhenHit;
                _enemyCharacterScript.DecreaseHp(5);
                _isAttacked = true;
            }
            
            // Reference : https://wlsdn629.tistory.com/entry/Lerp%EB%A5%BC-Pro%EC%B2%98%EB%9F%BC-%EC%9E%91%EC%84%B1%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95#google_vignette
            float duration = CharacterAnimator.GetCurrentAnimationDuration(CharacterAnimator.Layer.UpperLayer);
            float time = Mathf.Sin( duration * Mathf.PI * 0.5f);
            
            float positionX = Mathf.Lerp(
                _playerPositionWhenStartingThisState.x,
                _playerPositionWhenStartingThisState.x + _positionXAfterAttack,
                time);
            
            Vector3 characterPosition = CharacterTransform.position;
            characterPosition.x = positionX;
            this.CharacterTransform.position = characterPosition;
            
            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            
        }
    }
}