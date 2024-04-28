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

        public ReplayingMode(PlayerCharacter character)
            : base(PlayerModeManager.PlayerMode.Replaying, character)
        {
            _frameManager = GameObject.FindObjectOfType<FrameManager>();
            _enemyCharacter = character.EnemyObject.GetComponent<PlayerCharacter>();
            _judgeBoxController = character.GetComponent<CharacterJudgeBoxController>();
        }
        
        public override void Update()
        {
            _judgeBoxController.DisableHitBox();
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
                        if (PlayerStateCheckingMethodSet.IsGuardedState(nextState) ||
                            PlayerStateCheckingMethodSet.IsHitState(nextState)) 
                            Character.StateManager.ChangeState(nextState);
                        
                        _frameManager.PauseAllCharactersInFrame(FrameManager.PauseFrameWhenHit);
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
            base.Quit();
        }
    }
}