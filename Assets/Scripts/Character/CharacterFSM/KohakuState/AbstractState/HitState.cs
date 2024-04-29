using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public abstract class HitState : BehaviorStateInterface
    {
        private SoundManager _soundManager;

        public HitState(GameObject characterRoot, BehaviorEnumSet.State hitStateName,
            PassiveStateEnumSet.CharacterPositionState position) :
            base(hitStateName, characterRoot, BehaviorEnumSet.AttackLevel.Hit, position)
        {
            _soundManager = GameObject.FindObjectOfType<SoundManager>();
        }
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            _soundManager.PlayEffect(SoundManager.SoundSet.Hit);
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