namespace BehaviorTree
{
    public abstract class Node
    {
        public enum ResultNode
        {
            None,
            Success,
            Failure,
            Running
        }

        public ResultNode CurrentResult { get; protected set; }
        public int _nodeId;
        
        protected Node(int nodeId)
        {
            CurrentResult = ResultNode.None;
            _nodeId = nodeId;
        }
        
        public virtual void SetResult(ResultNode result) => CurrentResult = result;

        public abstract ResultNode Evaluate();
    }
}