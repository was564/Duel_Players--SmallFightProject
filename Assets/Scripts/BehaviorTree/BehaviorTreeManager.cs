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
            // MakeFirstTree();
            MakeSecondTree();
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
        
        private void MakeFirstTree()
        {
            _rootNode = new RootNode(0);
            addNode(_rootNode);
            addNode(new SelectorNode(1));
            setChild(0, 1); // root -> selector
            
            addNode(new IfNode(2, () => Math.Abs(_player.transform.position.x - _enemy.transform.position.x) > 2.5f));
            setChild(1, 2); // selector -> if
            addNode(new StateExecutionNode(3, BehaviorEnumSet.State.DashOnGround, _player));
            setChild(2, 3); // if -> state
            
            addNode(new IfNode(4, () => Math.Abs(_player.transform.position.x - _enemy.transform.position.x) > 1.5f));
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

            addNode(new IfNode(16, () => Math.Abs(_player.transform.position.x - _enemy.transform.position.x) < 0.9f));
            setChild(13, 16); // random selector -> if
            addNode(new StateExecutionNode(17, BehaviorEnumSet.State.BackStepOnGroundState, _player));
            setChild(16, 17); // if -> state
        }

        private void MakeSecondTree()
        {
            _rootNode = new RootNode(0);
            addNode(_rootNode);
            
            addNode(new SelectorNode(1));
            setChild(0, 1); // root -> selector
            
            addNode(new IfNode(2, () => PlayerStateCheckingMethodSet.IsAttackState(_enemy.StateManager.CurrentState.StateName)));
            setChild(1, 2); // selector -> if
            addNode(new RandomSelectorNode(3));
            setChild(2, 3); // if -> random selector

            addNode(new StateExecutionNode(4, BehaviorEnumSet.State.StandingPunch623Skill, _player));
            setChild(3, 4); // random selector -> state
            addNode(new SelectorNode(5));
            setChild(3, 5); // random selector -> selector
            
            addNode(new IfNode(6, () => _enemyJudgeBoxController.
                GetAttackBox(_enemy.StateManager.CurrentState.StateName).AttackPosition == BehaviorEnumSet.AttackPosition.Crouch));
            setChild(5, 6); // selector -> if
            addNode(new StateExecutionNode(7, BehaviorEnumSet.State.CrouchGuard, _player));
            setChild(6, 7); // if -> state
            addNode(new StateExecutionNode(8, BehaviorEnumSet.State.StandingGuard, _player));
            setChild(5, 8); // selector -> state

            addNode(new SequenceNode(9));
            setChild(1, 9); // selector -> sequence

            addNode(new TrueReturnNode(10));
            setChild(9, 10); // sequence -> true return

            addNode(new SequenceNode(11));
            setChild(10, 11); // true return -> sequence
            addNode(new SelectorNode(12));
            setChild(11, 12); // sequence -> selector
            
            addNode(new IfNode(13, () => Math.Abs(_player.transform.position.x - _enemy.transform.position.x) > 3.0f));
            setChild(12, 13); // selector -> if
            addNode(new StateExecutionNode(14, BehaviorEnumSet.State.DashOnGround, _player));
            setChild(13, 14); // if -> state
            
            addNode(new IfNode(15, () => Math.Abs(_player.transform.position.x - _enemy.transform.position.x) > 1.5f));
            setChild(12, 15); // selector -> if
            addNode(new StateExecutionNode(16, BehaviorEnumSet.State.Forward, _player));
            setChild(15, 16); // if -> state
            
            addNode(new WaitUntilIfTrueNode(17, _player, () => Math.Abs(_player.transform.position.x - _enemy.transform.position.x) < 0.7f));
            setChild(11, 17); // Sequence -> wait until if true

            addNode(new SelectorByGradientNode(23));
            setChild(9, 23); // sequence -> selector by gradient
            
            addNode(new RandomSelectorNode(18));
            setChild(23, 18); // Sequence -> random selector
            addNode(new StateExecutionNode(19, BehaviorEnumSet.State.StandingKick, _player));
            setChild(18, 19); // random selector -> state
            addNode(new StateExecutionNode(20, BehaviorEnumSet.State.StandingPunch, _player));
            setChild(18, 20); // random selector -> state

            addNode(new IfNode(21, () => Math.Abs(_player.transform.position.x - _enemy.transform.position.x) < 0.9f));
            setChild(18, 21); // random selector -> if
            addNode(new StateExecutionNode(22, BehaviorEnumSet.State.BackStepOnGroundState, _player));
            setChild(21, 22); // if -> state

            addNode(new StateExecutionNode(24, BehaviorEnumSet.State.GrabStart, _player));
            setChild(23, 24); // selector by gradient -> state
        }
    }
}