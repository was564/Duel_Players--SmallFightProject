using System;
using System.Collections.Generic;
using Character.CharacterFSM;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviorTreeManager
    {
        private RootNode _rootNode;
        
        private Dictionary<int, Node> _nodes;
        
        private PlayerCharacter _player;
        private CharacterJudgeBoxController _playerJudgeBoxController;
        private PlayerCharacter _enemy;
        private CharacterJudgeBoxController _enemyJudgeBoxController;

        public BehaviorTreeManager(PlayerCharacter player)
        {
            _nodes = new Dictionary<int, Node>();
            _player = player;
            _playerJudgeBoxController = _player.GetComponent<CharacterJudgeBoxController>();
            _enemy = player.EnemyObject.GetComponent<PlayerCharacter>();
            _enemyJudgeBoxController = _enemy.GetComponent<CharacterJudgeBoxController>();
            _rootNode = new RootNode(0);
            addNode(_rootNode);
            addNode(new SelectorNode(1));
            setChild(0, 1); // root -> selector
            
            addNode(new IfNode(2, () => Math.Abs(_player.transform.position.x - _enemy.transform.position.x) > 2.0f));
            setChild(1, 2); // selector -> if
            addNode(new StateExecutionNode(3, BehaviorEnumSet.State.DashOnGround, _player));
            setChild(2, 3); // if -> state
            
            addNode(new IfNode(4, () => Math.Abs(_player.transform.position.x - _enemy.transform.position.x) > 1.0f));
            setChild(1, 4); // selector -> if
            addNode(new StateExecutionNode(5, BehaviorEnumSet.State.Forward, _player));
            setChild(4, 5); // if -> state
            
            addNode(new IfNode(6, () => PlayerStateCheckingMethodSet.IsAttackState(_enemy.StateManager.CurrentState.StateName)));
            setChild(1, 6); // selector -> if
            addNode(new RandomSelectorNode(7));
            setChild(6, 7); // if -> random selector

            addNode(new StateExecutionNode(8, BehaviorEnumSet.State.StandingPunch623Skill, _player));
            setChild(7, 8); // random selector -> state
            addNode(new SelectorNode(9));
            setChild(7, 9); // random selector -> selector
            
            addNode(new IfNode(10, () => _enemyJudgeBoxController.
                    GetAttackBox(_enemy.StateManager.CurrentState.StateName).AttackPosition == BehaviorEnumSet.AttackPosition.Crouch));
            setChild(9, 10); // selector -> if
            addNode(new StateExecutionNode(11, BehaviorEnumSet.State.CrouchGuard, _player));
            setChild(10, 11); // if -> state
            addNode(new StateExecutionNode(12, BehaviorEnumSet.State.StandingGuard, _player));
            setChild(9, 12); // selector -> state

            addNode(new RandomSelectorNode(13));
            setChild(1, 13); // selector -> random selector
            addNode(new StateExecutionNode(14, BehaviorEnumSet.State.StandingKick, _player));
            setChild(13, 14); // random selector -> state
            addNode(new StateExecutionNode(15, BehaviorEnumSet.State.StandingPunch, _player));
            setChild(13, 15); // random selector -> state

            addNode(new IfNode(16, () => Math.Abs(_player.transform.position.x - _enemy.transform.position.x) < 0.4f));
            setChild(13, 16); // random selector -> if
            addNode(new StateExecutionNode(17, BehaviorEnumSet.State.BackStepOnGroundState, _player));
            setChild(16, 17); // if -> state
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
            if (parentNode is StateExecutionNodeByButton executionNode)
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