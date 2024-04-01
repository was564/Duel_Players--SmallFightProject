using Character.CharacterFSM;
using Character.PlayerMode;

namespace BehaviorTree
{
    public class StateExecutionNode : Node
    {
        private BehaviorEnumSet.State _executionState;

        private PlayerCharacter _player;
        
        private BehaviorStateSetInterface _stateSet;
        
        public StateExecutionNode(int nodeId, BehaviorEnumSet.State executionState, PlayerCharacter player) : base(nodeId)
        {
            _executionState = executionState;
            _player = player;
            _stateSet = _player.StateManager.StateSet;
        }

        /*
        private bool CheckState(BehaviorEnumSet.State currentState)
        {
            return (_player.StateManager.CurrentState.StateName == currentState);
        }
        */
        
        public override ResultNode Evaluate()
        {
            switch (CurrentResult)
            {
                case ResultNode.Success:
                    return ResultNode.Success;
                case ResultNode.Failure:
                    return ResultNode.Failure;
                default:
                    break;
            }

            if(_player.GetPlayerMode() == PlayerModeManager.PlayerMode.FramePause)
            {
                SetResult(ResultNode.Running);
                return ResultNode.Running;
            }

            int nextStateAttackLevel = _stateSet.GetStateInfo(_executionState).AttackLevel;
            int currentStateAttackLevel = _stateSet.GetStateInfo(_player.StateManager.CurrentState.StateName).AttackLevel;
            if(currentStateAttackLevel > nextStateAttackLevel)
            {
                SetResult(ResultNode.Failure);
                return ResultNode.Failure;
            }
            
            if(!PlayerStateCheckingMethodSet.CheckState(_player.StateManager.CurrentState.StateName, _executionState))
                _player.StateManager.ChangeState(_executionState);
            
            if (PlayerStateCheckingMethodSet.CheckState(_player.StateManager.CurrentState.StateName, _executionState))
            {
                SetResult(ResultNode.Success);
                return ResultNode.Success;
            }
            else
            {
                SetResult(ResultNode.Failure);
                return ResultNode.Failure;
            }
        }
        
        
    }
}