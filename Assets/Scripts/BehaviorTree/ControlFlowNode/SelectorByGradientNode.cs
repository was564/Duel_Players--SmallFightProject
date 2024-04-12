using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class SelectorByGradientNode : CompositeNode
    {
        protected List<float> _gradientList = new List<float>();
        
        public SelectorByGradientNode(int nodeId) : base(nodeId)
        {
            
        }
        
        public override ResultNode Evaluate()
        {
            int maxRand = 0;
            _gradientList.ForEach(gradient => maxRand += (int)gradient);
            int randGradient = UnityEngine.Random.Range(0, maxRand);

            int borderGradient = 0;
            for (int i = 0; i < _gradientList.Count; i++)
            {
                borderGradient += (int)_gradientList[i];
                if (randGradient <= borderGradient)
                {
                    CurrentResult = _children[i].Evaluate();
                    return CurrentResult;
                }
            }
            
            CurrentResult = ResultNode.Failure;
            return CurrentResult;
        }
        
        public override void AddChild(Node child)
        {
            _children.Add(child);
            _gradientList.Add((int)Mathf.Pow(_children.Count, 2));
        }

        public override void AddChild(Node child, int index)
        {
            _children.Insert(index, child);
            _gradientList.Add((int)Mathf.Pow(_children.Count, 2));
        }
        
        public override void RemoveChild(int nodeId)
        {
            for (int i = 0; i < _children.Count; i++)
                if (_children[i]._nodeId == nodeId)
                {
                    _children.RemoveAt(i);
                    _gradientList.RemoveAt(i);
                    break;
                }
        }
        
        /*
        private void AdjustGradient(int resultChildIndex)
        {
            if (resultChildIndex >= _gradientList.Count) return;
            for(int index = 0; index < _gradientList.Count; index++)
            {
                if (index == resultChildIndex)
                    continue;
                
                _gradientList[index] += 1;
            }
        }
        */
    }
}