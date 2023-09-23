using UnityEngine;

namespace Character.CharacterFSM
{
    public class StandingIdleState : BehaviorStateInterface
    {
        public StandingIdleState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.Idle, characterRoot) {}
        
        public override void Enter()
        {
            
        }

        public override void StateUpdate()
        {
            
        }

        public override void Quit()
        {
            
        }
    }
}