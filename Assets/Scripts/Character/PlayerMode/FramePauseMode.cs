namespace Character.PlayerMode
{
    public class FramePauseMode : PlayerModeInterface
    {
        public FramePauseMode(PlayerCharacter character) 
            : base(PlayerModeManager.PlayerMode.FramePause, character) { }
        
        public override void Update()
        {
            if(Character.GetPreviousPlayerMode() == PlayerModeManager.PlayerMode.NormalPlaying)
                Character.DecideBehaviorByInput();
            Character.UpdatePassiveState();
        }
    }
}