namespace BehaviorTree
{
    public class TrueReturnNode : DecoratorNode
    {
        public TrueReturnNode(int nodeId) : base(nodeId)
        { }
        
        public override ResultNode Evaluate()
        {
            switch (CurrentResult)
            {
                case ResultNode.Success:
                    return ResultNode.Success;
                case ResultNode.Failure:
                    return ResultNode.Failure;
                case ResultNode.Running:
                    break;
                default:
                    break;
            }
            
            ResultNode result = GetChild().Evaluate();
            if (result == ResultNode.Running)
            {
                SetResult(ResultNode.Running);
                return ResultNode.Running;
            }
            else
            {
                SetResult(ResultNode.Success);
                return ResultNode.Success;
            }
        }
    }
}