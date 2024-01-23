using System.Collections.Generic;

namespace Character.PlayerMode
{
    public class PlayerModeManager
    {
        public enum PlayerMode
        {
            NormalPlaying = 0,
            Replaying,
            GamePause,
            FramePause,
            Size
        }
        
        private Dictionary<PlayerMode, PlayerModeInterface> _playerModes 
            = new Dictionary<PlayerMode, PlayerModeInterface>();
        
        private PlayerModeInterface _currentMode;
        
        public PlayerModeManager(PlayerCharacter character)
        {
            InitPlayerModes(character);
            _currentMode = _playerModes[PlayerMode.NormalPlaying];
        }
        
        public void SetMode(PlayerMode mode)
        {
            _currentMode = _playerModes[mode];
        }
        
        public void Update()
        {
            _currentMode.Update();
        }
        
        private void InitPlayerModes(PlayerCharacter character)
        {
            _playerModes.Add(PlayerMode.NormalPlaying, new NormalPlayingMode(character));
            _playerModes.Add(PlayerMode.Replaying, new ReplayingMode(character));
            _playerModes.Add(PlayerMode.GamePause, new GamePauseMode(character));
            _playerModes.Add(PlayerMode.FramePause, new FramePauseMode(character));
        }
    }
}