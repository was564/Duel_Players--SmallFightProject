namespace Character.PlayerMode
{
    public class ReplayingMode : PlayerModeInterface
    {
        public ReplayingMode(PlayerCharacter character) 
            : base(PlayerModeManager.PlayerMode.Replaying, character) { }
        
        public override void Update()
        {
            //Character.UpdateReplaying();
        }
    }
}