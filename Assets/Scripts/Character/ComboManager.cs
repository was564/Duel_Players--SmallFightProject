using System.Collections.Generic;
using System.Linq;
using Character.CharacterFSM;
using Unity.VisualScripting;
using UnityEngine;

namespace Character
{
    public class ComboManager
    {
        private Dictionary<BehaviorEnumSet.State, int> _limitStatesCancelCountInTheCombo =
            new Dictionary<BehaviorEnumSet.State, int>();

        private Dictionary<BehaviorEnumSet.State, int> _countStatesCancel =
            new Dictionary<BehaviorEnumSet.State, int>();

        public bool IsDoingCombo { get; set; }

        private PlayerCharacter _player;

        private PlayerCharacter _enemyCharacter;

        public bool IsCanceled { get; set; } = false;
        
        public ComboManager(PlayerCharacter player)
        {
            _player = player;
            _enemyCharacter = player.EnemyObject.GetComponent<PlayerCharacter>();
            
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.Jump, 1);
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.StandingPunch, 1);
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.CrouchPunch, 1);
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.AiringPunch, 1);
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.AiringKick, 1);
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.CrouchKick, 1);
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.StandingKick, 1);
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.StandingPunch236Skill, 1);
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.StandingKick236Skill, 1);
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.StandingPunch623Skill, 1);
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.StandingKick623Skill, 1);
            
            _countStatesCancel.Add(BehaviorEnumSet.State.Jump, 0);
            _countStatesCancel.Add(BehaviorEnumSet.State.StandingPunch, 0);
            _countStatesCancel.Add(BehaviorEnumSet.State.CrouchPunch, 0);
            _countStatesCancel.Add(BehaviorEnumSet.State.AiringPunch, 0);
            _countStatesCancel.Add(BehaviorEnumSet.State.AiringKick, 0);
            _countStatesCancel.Add(BehaviorEnumSet.State.CrouchKick, 0);
            _countStatesCancel.Add(BehaviorEnumSet.State.StandingKick, 0);
            _countStatesCancel.Add(BehaviorEnumSet.State.StandingPunch236Skill, 0);
            _countStatesCancel.Add(BehaviorEnumSet.State.StandingKick236Skill, 0);
            _countStatesCancel.Add(BehaviorEnumSet.State.StandingPunch623Skill, 0);
            _countStatesCancel.Add(BehaviorEnumSet.State.StandingKick623Skill, 0);
        }

        public void CountStateCancel(BehaviorEnumSet.State state)
        {
            if (!_countStatesCancel.ContainsKey(state)) return;
            _countStatesCancel[state]++;
            IsCanceled = true;
        }

        public bool CheckStateTransition(BehaviorStateInterface currentState, BehaviorStateInterface nextState)
        {
            BehaviorEnumSet.AttackLevel currentAttackLevel = (BehaviorEnumSet.AttackLevel)currentState.AttackLevel;
            BehaviorEnumSet.AttackLevel nextAttackLevel = (BehaviorEnumSet.AttackLevel)nextState.AttackLevel;
            
            if (_countStatesCancel.ContainsKey(nextState.StateName) &&
                (_countStatesCancel[nextState.StateName] >= _limitStatesCancelCountInTheCombo[nextState.StateName]))
                return false;
            
            switch (currentAttackLevel)
            {
                case BehaviorEnumSet.AttackLevel.Move:
                    return true;
                    break;
                case BehaviorEnumSet.AttackLevel.BasicAttack:
                    if (nextAttackLevel >= BehaviorEnumSet.AttackLevel.CancelableMove) return true;
                    break;
                case BehaviorEnumSet.AttackLevel.Technique:
                    if (nextAttackLevel > BehaviorEnumSet.AttackLevel.SpecialMove) return true;
                    break;
                case BehaviorEnumSet.AttackLevel.Hit:
                    if (nextAttackLevel == BehaviorEnumSet.AttackLevel.Hit) return true;
                    break;
                default:
                    break;
            }

            IsCanceled = false;
            return false;
        }
        
        public bool TryActivateSkillState(BehaviorEnumSet.Behavior input, BehaviorStateSimulator stateManager)
        {
            if (!_enemyCharacter.IsHitContinuous) return false;
            
            BehaviorEnumSet.State nextState = BehaviorEnumSet.State.Null;
            switch (input) // AttackLevel이 Cancelable Move 이상부터인 스킬 넣기
            {
                case BehaviorEnumSet.Behavior.StandingPunch236Skill:
                    nextState = BehaviorEnumSet.State.StandingPunch236Skill;
                    break;
                case BehaviorEnumSet.Behavior.Dash:
                    nextState = BehaviorEnumSet.State.DashOnGround;
                    break;
                case BehaviorEnumSet.Behavior.Jump:
                    nextState = BehaviorEnumSet.State.Jump;
                    break;
                case BehaviorEnumSet.Behavior.StandingKick236Skill:
                    nextState = BehaviorEnumSet.State.StandingKick236Skill;
                    break;
                case BehaviorEnumSet.Behavior.StandingPunch623Skill:
                    nextState = BehaviorEnumSet.State.StandingPunch623Skill;
                    break;
                case BehaviorEnumSet.Behavior.StandingKick623Skill:
                    nextState = BehaviorEnumSet.State.StandingKick623Skill;
                    break;
                default:
                    break;
            }

            if (nextState != BehaviorEnumSet.State.Null &&
                CheckStateTransition(stateManager.CurrentState, stateManager.GetStateInfo(nextState)))
            {
                stateManager.ChangeState(nextState);
                CountStateCancel(nextState);
                return true;
            }
            return false;
        }

        public void UpdateComboManager()
        {
            if (!_enemyCharacter.IsHitContinuous)
            {
                var keys = _countStatesCancel.Keys.ToList();
                for (int index = 0; index < keys.Count; index++)
                {
                    _countStatesCancel[keys[index]] = 0;
                }
                
                IsCanceled = false;
            }
        }
    }
}