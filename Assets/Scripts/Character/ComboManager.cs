using System.Collections.Generic;
using Character.CharacterFSM;
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

        private BehaviorStateSimulator _stateManager;
        
        public ComboManager()
        {
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

        public bool CheckStateTransition(BehaviorStateInterface currentState, BehaviorStateInterface nextState)
        {
            BehaviorEnumSet.AttackLevel currentAttackLevel = (BehaviorEnumSet.AttackLevel)currentState.AttackLevel;
            BehaviorEnumSet.AttackLevel nextAttackLevel = (BehaviorEnumSet.AttackLevel)nextState.AttackLevel;

            switch (currentAttackLevel)
            {
                case BehaviorEnumSet.AttackLevel.Move:
                    return true;
                    break;
                case BehaviorEnumSet.AttackLevel.BasicAttack:
                    if (nextAttackLevel >= BehaviorEnumSet.AttackLevel.BasicAttack) return true;
                    break;
                case BehaviorEnumSet.AttackLevel.Technique:
                    if (nextAttackLevel > BehaviorEnumSet.AttackLevel.SpecialMove) return true;
                    break;
                default:
                    break;
            }
            
            return false;
            
        }

        // ComboManager가 attackLevel이 BasicAttack이상인 State들의 이동을 관리하려 했으나
        // State에서 가독성이 많이 떨어지고 ComboManager가 StateManager의 역할이 중복된다.
        // 대안 : StateManager가 State의 Transition을 실행할 때 검증 역할
        /*
        public bool TryActivateSkillState(BehaviorEnumSet.Behavior input, BehaviorStateSimulator stateManager)
        {
            bool result = true;
            switch (input)
            {
                case BehaviorEnumSet.Behavior.StandingPunchSkill:
                    stateManager.ChangeState(BehaviorEnumSet.State.StandingPunchSkill);
                    break;
                case BehaviorEnumSet.Behavior.Dash:
                    stateManager.ChangeState(BehaviorEnumSet.State.DashOnGround);
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }
        */
        
        public BehaviorEnumSet.State GetNextStateByInput(BehaviorEnumSet.State currentState, BehaviorEnumSet.Behavior behavior)
        {
            
            
            return currentState;
        }
    }
}