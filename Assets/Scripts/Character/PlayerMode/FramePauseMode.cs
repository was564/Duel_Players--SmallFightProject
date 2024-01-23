namespace Character.PlayerMode
{
    public class FramePauseMode : PlayerModeInterface
    {
        public FramePauseMode(PlayerCharacter character) : base(character) { }
        
        public override void Update()
        {
            Character.DecideBehaviorByInput();
            Character.UpdatePassiveState();
        }
    }
}