namespace BehaviorTree
{
    public class SequenceNode : CompositeNode
    {
        public SequenceNode(int nodeId)
            : base(nodeId)
        { }
        
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
            
            foreach (var child in _children)
            {
                switch (child.Evaluate())
                {
                    case ResultNode.Success:
                        continue;
                    case ResultNode.Running:
                        SetResult(ResultNode.Running);
                        return ResultNode.Running;
                    case ResultNode.Failure:
                        SetResult(ResultNode.Failure);
                        return ResultNode.Failure;
                }
            }
            
            SetResult(ResultNode.Success);
            return ResultNode.Success;
        }
    }
}