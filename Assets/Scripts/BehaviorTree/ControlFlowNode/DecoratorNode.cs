using System.Collections.Generic;

namespace BehaviorTree
{
    public abstract class DecoratorNode : Node
    {
        private Node _child;

        protected DecoratorNode(int nodeId)
            : base(nodeId)
        {
            // 추후에 null State 만들기2
            _child = null;
        }
        
        public Node GetChild() => _child;
        
        public void SetChild(Node child) => _child = child;

        public void RemoveChild() => _child = null;

        public override void SetResult(Node.ResultNode result)
        {
            CurrentResult = result;
            if (result == Node.ResultNode.Running) return;
            _child.SetResult(ResultNode.None);
        }

    }
}