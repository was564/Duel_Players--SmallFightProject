using System.Collections.Generic;
using System.Linq;
using Character.CharacterFSM;
using TMPro;
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

        private BehaviorStateSetInterface _stateSet;
        
        private PlayerCharacter _player;

        private PlayerCharacter _enemyCharacter;
        
        private CharacterAnimator _enemyCharacterAnimator;
        
        private TextMeshProUGUI _text;

        private int _comboCount;
        
        public bool IsCanceled { get; set; } = false;
        
        public ComboManager(PlayerCharacter player)
        {
            _comboCount = 0;
            
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
            _limitStatesCancelCountInTheCombo.Add(BehaviorEnumSet.State.StandingPunch6246SpecialSkillEnter, 1);
            
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
            _countStatesCancel.Add(BehaviorEnumSet.State.StandingPunch6246SpecialSkillEnter, 0);
        }

        public void Initialize(BehaviorStateSetInterface stateSet)
        {
            _stateSet = stateSet;
            _enemyCharacterAnimator = _enemyCharacter.GetComponent<CharacterAnimator>();
            foreach (var text in GameObject.FindObjectsOfType<TextMeshProUGUI>())
            {
                if (text.CompareTag(_player.tag) && LayerMask.NameToLayer("InGameUI") == text.gameObject.layer)
                    _text = text;
            }
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
                case BehaviorEnumSet.AttackLevel.BasicAttack:
                    if (nextAttackLevel >= BehaviorEnumSet.AttackLevel.CancelableMove) return true;
                    break;
                case BehaviorEnumSet.AttackLevel.Technique:
                    if (nextAttackLevel > BehaviorEnumSet.AttackLevel.SpecialMove) return true;
                    break;
                case BehaviorEnumSet.AttackLevel.Hit:
                    if (nextAttackLevel == BehaviorEnumSet.AttackLevel.Hit) return true;
                    break;
            }

            IsCanceled = false;
            return false;
        }
        
        public BehaviorEnumSet.State TryGetActivateSkillState(BehaviorEnumSet.State currentState, BehaviorEnumSet.Behavior input)
        {
            if (!_enemyCharacter.IsHitContinuous) return BehaviorEnumSet.State.Null;
            
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
                case BehaviorEnumSet.Behavior.StandingPunch6246SpecialSkill:
                    nextState = BehaviorEnumSet.State.StandingPunch6246SpecialSkillEnter;
                    break;
                default:
                    nextState = BehaviorEnumSet.State.Null;
                    break;
            }

            if (nextState != BehaviorEnumSet.State.Null &&
                CheckStateTransition(_stateSet.GetStateInfo(currentState), _stateSet.GetStateInfo(nextState)))
            {
                CountStateCancel(nextState);
                return nextState;
            }
            else
            {
                return BehaviorEnumSet.State.Null;
            }
        }

        private float _enemyAnimationDuration = 2f;
        public void UpdateComboManager()
        {
            if (!_enemyCharacter.IsHitContinuous)
            {
                var keys = _countStatesCancel.Keys.ToList();
                for (int index = 0; index < keys.Count; index++)
                {
                    _countStatesCancel[keys[index]] = 0;
                }

                _comboCount = 0;
                _enemyAnimationDuration = 0;
                IsCanceled = false;
                return;
            }
        
            if (PlayerStateCheckingMethodSet.IsHittedState(_enemyCharacter.StateManager.CurrentState.StateName))
            {
                float duration = _enemyCharacterAnimator.GetCurrentAnimationDuration(CharacterAnimator.Layer.UpperLayer);
                if(duration < _enemyAnimationDuration)
                {
                    _enemyAnimationDuration = duration;
                    _comboCount++;
                    _text.text = _comboCount + " Combo";
                }
            }
        }
    }
}