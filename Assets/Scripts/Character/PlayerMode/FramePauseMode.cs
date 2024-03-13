namespace Character.PlayerMode
{
    public class FramePauseMode : PlayerModeInterface
    {
        public FramePauseMode(PlayerCharacter character) 
            : base(PlayerModeManager.PlayerMode.FramePause, character) { }
        
        public override void Update()
        {
            Character.DecideBehaviorByInput();
            Character.UpdatePassiveState();
        }
    }
}