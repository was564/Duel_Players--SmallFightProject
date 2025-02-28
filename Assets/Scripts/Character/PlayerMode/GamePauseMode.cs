namespace Character.PlayerMode
{
    public class GamePauseMode : PlayerModeInterface
    {
        public GamePauseMode(PlayerCharacter character) 
            : base(PlayerModeManager.PlayerMode.GamePause, character) { }
        
        public override void Update()
        {
            return;
        }
    }
}