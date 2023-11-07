using System.Collections.Generic;
using Unity.VisualScripting;

namespace Character
{
    public class ComboManager
    {
        private Dictionary<BehaviorEnumSet.State, int> _limitStatesCancelCountInTheCombo =
            new Dictionary<BehaviorEnumSet.State, int>();

        private Dictionary<BehaviorEnumSet.State, int> _countStatesCancel =
            new Dictionary<BehaviorEnumSet.State, int>();

        public bool IsDoingCombo { get; set; }

        private BehaviorStateManager _stateManager;
        
        public ComboManager(BehaviorStateManager stateManager)
        {
            _stateManager = stateManager;
            
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.Jump, 1);
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.StandingPunch, 2);
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.CrouchPunch, 2);
            
            _countStatesCancel.Add(BehaviorEnumSet.State.Jump, 0);
            _countStatesCancel.Add(BehaviorEnumSet.State.StandingPunch, 0);
            _countStatesCancel.Add(BehaviorEnumSet.State.CrouchPunch, 0);
        }

        public void CountStateCancel(BehaviorEnumSet.State state)
        {
            _countStatesCancel[state]++;
        }

        public bool TryActivateSkillState(BehaviorEnumSet.Behavior input)
        {
            bool result = true;
            switch (input)
            {
                case BehaviorEnumSet.Behavior.StandingPunchSkill:
                    _stateManager.ChangeState(BehaviorEnumSet.State.StandingPunchSkill);
                    break;
                case BehaviorEnumSet.Behavior.Dash:
                    _stateManager.ChangeState(BehaviorEnumSet.State.DashOnGround);
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }
        
        public BehaviorEnumSet.State GetNextStateByInput(BehaviorEnumSet.State currentState, BehaviorEnumSet.Behavior behavior)
        {
            
            
            return currentState;
        }
    }
}