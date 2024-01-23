namespace Character.PlayerMode
{
    public abstract class PlayerModeInterface
    {
        protected PlayerCharacter Character;
        
        public PlayerModeInterface(PlayerCharacter character)
        {
            Character = character;
        }
        
        public abstract void Update();
    }
}