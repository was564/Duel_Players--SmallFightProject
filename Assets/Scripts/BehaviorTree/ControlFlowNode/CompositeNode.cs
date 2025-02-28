using System.Collections.Generic;

namespace BehaviorTree
{
    public abstract class CompositeNode : Node
    {
        protected List<Node> _children;

        protected CompositeNode(int nodeId)
            : base(nodeId)
        {
            _children = new List<Node>();
        }
        
        public List<Node> GetChildren() => _children;

        public virtual void AddChild(Node child) 
            => _children.Add(child);

        public virtual void AddChild(Node child, int index)
            => _children.Insert(index, child);

        public virtual void RemoveChild(int nodeId)
        {
            for (int i = 0; i < _children.Count; i++)
                if (_children[i]._nodeId == nodeId)
                {
                    _children.RemoveAt(i);
                    break;
                }
        }

        public override void SetResult(ResultNode result)
        {
            CurrentResult = result;
            if (result == ResultNode.Running) return;
            foreach (var child in _children)
                child.SetResult(ResultNode.None);
        }
        
        public void ClearChild() => _children.Clear();

    }
}