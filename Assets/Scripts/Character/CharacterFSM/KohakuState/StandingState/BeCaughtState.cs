using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class BeCaughtState : StandingStopHitState
    {
        public BeCaughtState(GameObject characterRoot) : base(characterRoot)
        {
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