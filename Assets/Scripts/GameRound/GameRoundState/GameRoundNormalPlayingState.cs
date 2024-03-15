using UnityEngine;

namespace GameRound
{
    public class GameRoundNormalPlayingState : GameRoundStateInterface
    {
        public GameRoundNormalPlayingState(GameRoundStateManager manager)
            : base(manager, GameRoundManager.GameState.NormalPlay) { }

        public override void Enter()
        {
            //Debug.Log(FrameManager.CurrentFrame);
            RoundStateManager.FrameManager.IsFramePaused = false;
            RoundManager.ApplySettingInStateByPausing(false);
        }
        
        public override void Update()
        {
            if (RoundManager.IsGameEnded)
                RoundStateManager.ChangeState(GameRoundManager.GameState.Wait);
            
            if(InputManager.IsPressedMenuKey())
                RoundStateManager.ChangeState(GameRoundManager.GameState.Pause);
            
            RoundManager.DecreaseRemainTimePerFrame(1);
        }
        
        public override void Quit()
        {
            RoundStateManager.FrameManager.IsFramePaused = true;
        }
    }
}