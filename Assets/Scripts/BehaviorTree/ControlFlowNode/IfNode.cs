using System;

namespace BehaviorTree
{
    public class IfNode : DecoratorNode
    {
        public delegate bool ConditionMethod();
        
        private ConditionMethod _conditionMethod;

        public IfNode(int nodeId, ConditionMethod delegateMethod)
            : base(nodeId)
        {
            _conditionMethod = delegateMethod;
        }
        
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

            if (CurrentResult == ResultNode.Running || _conditionMethod())
            {
                ResultNode result = GetChild().Evaluate();
                SetResult(result);
                return result;
            }
            else
            {
                SetResult(ResultNode.Failure);
                return ResultNode.Failure;
            }
        }
    }
}