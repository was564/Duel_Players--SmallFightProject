namespace Character.PlayerMode
{
    public abstract class PlayerModeInterface
    {
        protected PlayerCharacter Character;
        public PlayerModeManager.PlayerMode ModeName { get; private set; }
        
        public PlayerModeInterface(PlayerModeManager.PlayerMode mode, PlayerCharacter character)
        {
            ModeName = mode;
            Character = character;
        }
        
        public abstract void Update();
    }
}