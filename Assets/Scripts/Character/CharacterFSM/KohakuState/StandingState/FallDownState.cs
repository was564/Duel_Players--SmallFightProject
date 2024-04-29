using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class FallDownState : BehaviorStateInterface
    {
        private ParticleSystem _fallDownDustEffect;
        private SoundManager _soundManager;
        
        public FallDownState(GameObject characterRoot) :
            base(BehaviorEnumSet.State.FallDown, characterRoot,
                BehaviorEnumSet.AttackLevel.Hit, PassiveStateEnumSet.CharacterPositionState.OnGround)
        {
            _soundManager = GameObject.FindObjectOfType<SoundManager>();
            foreach (var childTransform in characterRoot.transform.GetComponentsInChildren<ParticleSystem>())
            {
                if (childTransform.tag.Equals("FallDownParticle"))
                    _fallDownDustEffect = childTransform.GetComponent<ParticleSystem>();
            }
            
            _fallDownDustEffect.Stop();
        }

        private int stateStartingFrame;
        [SerializeField]
        private int lyingDownFrame = 12;
        public override void Enter()
        {
            stateStartingFrame = 0;
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            CharacterJudgeBoxController.DisableHitBox();
             
            CharacterAnimator.PlayAnimation("FallDown", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimation("FallDown", CharacterAnimator.Layer.LowerLayer);
            
            _soundManager.PlayEffect(SoundManager.SoundSet.Fall);
            _fallDownDustEffect.Play();
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
            if (PlayerCharacter.Hp <= 0)
            {
                PlayerCharacter.IsEndedPoseAnimation = true;
                return BehaviorEnumSet.State.Null;
            }
            stateStartingFrame += 1;
            if (stateStartingFrame >= lyingDownFrame)
                return BehaviorEnumSet.State.GetUp;
            else return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            PlayerCharacter.IsHitContinuous = false;
        }
    }
}