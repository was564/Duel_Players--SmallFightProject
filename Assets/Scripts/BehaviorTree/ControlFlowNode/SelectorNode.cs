namespace BehaviorTree
{
    public sealed class SelectorNode : CompositeNode
    {
        public SelectorNode(int nodeId)
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
            
            foreach (var child in GetChildren())
            {
                switch (child.Evaluate())
                {
                    case ResultNode.Success:
                        SetResult(ResultNode.Success);
                        return ResultNode.Success;
                    case ResultNode.Running:
                        SetResult(ResultNode.Running);
                        return ResultNode.Running;
                    case ResultNode.Failure:
                        continue;
                }
            }

            SetResult(ResultNode.Failure);
            return ResultNode.Failure;
        }
    }
}