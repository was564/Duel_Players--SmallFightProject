using UnityEngine;

namespace BehaviorTree
{
    public class RandomSelectorNode : SelectorNode
    {
        public RandomSelectorNode(int nodeId)
            : base(nodeId)
        { }

        public override ResultNode Evaluate()
        {
            Shuffle();
            return base.Evaluate();
        }
        
        private void Shuffle()
        {
            for (int i = 0; i < _children.Count; i++)
            {
                int randomIndex = Random.Range(i, _children.Count);
                (_children[i], _children[randomIndex]) = (_children[randomIndex], _children[i]);
            }
        }
    }
}