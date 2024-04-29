using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class EscapeGrabState : BehaviorStateInterface
    {
        private SoundManager _soundManager;
        
        public EscapeGrabState(GameObject characterRoot) : base(BehaviorEnumSet.State.GrabEscape, characterRoot,
            BehaviorEnumSet.AttackLevel.SpecialMove, PassiveStateEnumSet.CharacterPositionState.OnGround)
        {
            _soundManager = GameObject.FindObjectOfType<SoundManager>();
        }

        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            _soundManager.PlayEffect(SoundManager.SoundSet.Grab);
            
            CharacterAnimator.PlayAnimation("StandingHit", CharacterAnimator.Layer.UpperLayer, true);
            CharacterAnimator.PlayAnimation("StandingHit", CharacterAnimator.Layer.LowerLayer, true);
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
            if (CharacterAnimator.IsEndCurrentAnimation("StandingHit", CharacterAnimator.Layer.UpperLayer))
                return BehaviorEnumSet.State.StandingIdle;

            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            
        }
    }
}