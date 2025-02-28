using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class LandState : BehaviorStateInterface
    {
        private ParticleSystem _landDustEffect;

        public LandState(GameObject characterRoot) :
            base(BehaviorEnumSet.State.Land, characterRoot,
                BehaviorEnumSet.AttackLevel.Move, PassiveStateEnumSet.CharacterPositionState.OnGround)
        {
            foreach (var childTransform in characterRoot.transform.GetComponentsInChildren<ParticleSystem>())
            {
                if (childTransform.tag.Equals("LandParticle"))
                    _landDustEffect = childTransform.GetComponent<ParticleSystem>();
            }
            
            _landDustEffect.Stop();
        }

        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            PlayerCharacter.LookAtEnemy();
            
            CharacterRigidBody.velocity = Vector3.zero;
            
            CharacterAnimator.PlayAnimationSmoothly("Land", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("Land", CharacterAnimator.Layer.LowerLayer);
            
            _landDustEffect.Play();
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
            if(CharacterAnimator.IsEndCurrentAnimation("Land", CharacterAnimator.Layer.LowerLayer))
                return BehaviorEnumSet.State.StandingIdle;
            else
            {
                /*
                Vector3 characterPosition = this.CharacterTransform.position;
                float positionY = Mathf.Lerp(
                    this.PlayerCharacter.PositionYOffsetForLand,
                    0.0f,
                    CharacterAnimator.GetCurrentAnimationDuration(CharacterAnimator.Layer.LowerLayer));
                characterPosition.y = positionY;
                this.CharacterTransform.position = characterPosition;
                */
            }

            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            PlayerCharacter.IsDoubleJumped = false;
            
            Vector3 characterPosition = this.CharacterTransform.position;
            characterPosition.y = 0;
            this.CharacterTransform.position = characterPosition;
        }
    }
}