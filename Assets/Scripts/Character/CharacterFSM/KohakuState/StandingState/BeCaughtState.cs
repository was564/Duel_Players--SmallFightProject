using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class BeCaughtState : StandingStopHitState
    {
        private SoundManager _soundManager;
        
        public BeCaughtState(GameObject characterRoot) : base(characterRoot)
        {
            _soundManager = GameObject.FindObjectOfType<SoundManager>();
        }

        public override void Enter()
        {
            base.Enter();
            
            _soundManager.PlayEffect(SoundManager.SoundSet.Grab);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Grab:
                    return BehaviorEnumSet.State.GrabEscape;
                
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }
    }
}