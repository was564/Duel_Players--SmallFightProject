using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviorTreeManager
    {
        private RootNode _rootNode;
        
        private Dictionary<int, Node> _nodes;
        
        private PlayerCharacter _player;

        public BehaviorTreeManager(PlayerCharacter player)
        {
            _nodes = new Dictionary<int, Node>();
            _player = player;
            _rootNode = new RootNode(0);
            addNode(_rootNode);
            addNode(new SelectorNode(1));
            addNode(new BackwardWalkingNode(2, _player));

            setChild(0, 1);
            setChild(1, 2);
        }
        
        private bool addNode(Node node)
        {
            if (_nodes.ContainsKey(node._nodeId)) return false;
            
            _nodes.Add(node._nodeId, node);
            return true;
        }
        
        private bool setChild(int parentNodeId, int childNodeId)
        {
            Node parentNode = _nodes[parentNodeId];
            Node childNode = _nodes[childNodeId];
            if (parentNode is CompositeNode compositeNode)
            {
                compositeNode.AddChild(childNode);
                return true;
            }
            if (parentNode is DecoratorNode decoratorNode)
            {
                decoratorNode.SetChild(childNode);
                return true;
            }
            if (parentNode is StateExecutionNode executionNode)
            {
                return false;
            }

            return false;
        }
        
        public Node.ResultNode BehaviorTreeUpdate()
        {
            return _rootNode.Evaluate();
        }
    }
}