namespace BehaviorTree
{
    public class RootNode : DecoratorNode
    {
        public RootNode(int nodeId) : base(nodeId) 
        {}
        
        public override ResultNode Evaluate()
        {
            ResultNode result = GetChild().Evaluate();
            SetResult(result);
            return result;
        }
    }
}