using System.Collections.Generic;
using Character.CharacterFSM;
using UnityEngine;

namespace Character.PlayerMode
{
    public class ReplayingMode : PlayerModeInterface
    {
        public Queue<EntryState> ReplayingInputQueue { get; set; }
        
        private PlayerCharacter _enemyCharacter;
        private FrameManager _frameManager;
        private CharacterJudgeBoxController _judgeBoxController;
        
        private CharacterJudgeBoxController _enemyJudgeBoxController;
        
        public ReplayingMode(PlayerCharacter character)
            : base(PlayerModeManager.PlayerMode.Replaying, character)
        {
            _frameManager = GameObject.FindObjectOfType<FrameManager>();
            _enemyCharacter = character.EnemyObject.GetComponent<PlayerCharacter>();
            _judgeBoxController = character.GetComponent<CharacterJudgeBoxController>();
            _enemyJudgeBoxController = _enemyCharacter.GetComponent<CharacterJudgeBoxController>();
        }
        
        public override void Enter()
        {
            _judgeBoxController.CanTurnOnAttackBox = false;
        }
        
        public override void Update()
        {
            UpdateForReplaying();
            Character.UpdatePassiveState();
            Character.StateManager.UpdateState();
        }
        
        private void UpdateForReplaying()
        {
            while (ReplayingInputQueue.Count > 0)
            {
                EntryState entry = ReplayingInputQueue.Peek();
                if(entry.Frame < FrameManager.CurrentFrame)
                {
                    ReplayingInputQueue.Dequeue();
                    continue;
                }
                else if (entry.Frame == FrameManager.CurrentFrame)
                {
                    ReplayingInputQueue.Dequeue();
                    if((PlayerCharacter.CharacterIndex)entry.CharacterIndex != Character.PlayerUniqueIndex) 
                        continue;

                    BehaviorEnumSet.State nextState = (BehaviorEnumSet.State)entry.State;
                    
                    Character.transform.position = new Vector3(entry.PositionX, entry.PositionY);
                    Character.RigidBody.velocity = new Vector3(entry.VelocityX, entry.VelocityY);
                    Character.DecreaseHp(Character.Hp - entry.Hp);
                    
                    if (entry.FrameStopped) 
                    {
                        if (PlayerStateCheckingMethodSet.IsGuardedState(nextState))
                        {
                            Character.StateManager.ChangeState(nextState);
                            _enemyJudgeBoxController.GetAttackBox(_enemyCharacter.StateManager.CurrentState.StateName).PlayGuardEffect();
                        }
                        else if (PlayerStateCheckingMethodSet.IsHitState(nextState))
                        {
                            Character.StateManager.ChangeState(nextState);
                            _enemyJudgeBoxController.GetAttackBox(_enemyCharacter.StateManager.CurrentState.StateName).PlayHitEffect();
                        }
                        
                        _frameManager.PauseAllCharactersInFrame(FrameManager.PauseFrameWhenHit - 1);
                    }
                    else Character.StateManager.ChangeState(nextState);
                    
                    break;
                }
                else
                {
                    break;
                }
            }
        }
        
        public override void Quit()
        {
            _judgeBoxController.CanTurnOnAttackBox = true;
        }
    }
}