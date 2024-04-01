using System.Collections.Generic;
using Character.PlayerMode;
using UnityEngine.InputSystem;

namespace BehaviorTree
{
    public abstract class StateExecutionNodeByButton : Node
    {
        public BehaviorEnumSet.State GoalState { get; private set; }

        private List<BehaviorEnumSet.Button> _inputList;

        private Queue<BehaviorEnumSet.Button> _inputQueue;

        private PlayerCharacter _player;
        private CharacterInputManager _inputManager;
        
        protected StateExecutionNodeByButton(int nodeId, BehaviorEnumSet.State goalState,
            PlayerCharacter player, List<BehaviorEnumSet.Button> buttonList)
            : base(nodeId)
        {
            GoalState = goalState;
            _inputManager = player.GetComponent<CharacterInputManager>();
            _player = player;
            _inputList = buttonList;
            _inputQueue = new Queue<BehaviorEnumSet.Button>();
        }
        
        protected bool CheckGoalState(BehaviorEnumSet.State currentState)
        {
            return (GoalState == currentState);
        }
        
        public void SetInputList(List<BehaviorEnumSet.Button> inputList)
        {
            _inputList = inputList;
        }
        
        private void EnqueueInputs()
        {
            foreach (var input in _inputList)
                _inputQueue.Enqueue(input);
        }
        
        public sealed override ResultNode Evaluate()
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

            if (CurrentResult == ResultNode.None)
            {
                _inputQueue.Clear();
                EnqueueInputs();
            }

            if(_player.GetPlayerMode() == PlayerModeManager.PlayerMode.FramePause)
            {
                SetResult(ResultNode.Running);
                return ResultNode.Running;
            }
            
            if(_inputQueue.Count == 0){
                if (CheckGoalState(_player.StateManager.CurrentState.StateName))
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
            else
            {
                BehaviorEnumSet.Button input = _inputQueue.Dequeue();
                _inputManager.EnqueueInput(input);
                SetResult(ResultNode.Running);
                return ResultNode.Running;
            }
        }
    }
}