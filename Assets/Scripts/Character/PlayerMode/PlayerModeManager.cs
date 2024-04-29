using System.Collections.Generic;

namespace Character.PlayerMode
{
    public class PlayerModeManager
    {
        public enum PlayerMode
        {
            NormalPlaying = 0,
            Replaying,
            AI,
            GamePause,
            FramePause,
            Size
        }
        
        private Dictionary<PlayerMode, PlayerModeInterface> _playerModes 
            = new Dictionary<PlayerMode, PlayerModeInterface>();
        
        private PlayerModeInterface _currentMode;
        
        private PlayerMode _previousMode;
        
        public PlayerModeManager(PlayerCharacter character)
        {
            InitPlayerModes(character);
            _currentMode = _playerModes[PlayerMode.NormalPlaying];
            _previousMode = PlayerMode.NormalPlaying;
        }
        
        public void SetMode(PlayerMode mode)
        {
            _currentMode.Quit();
            _previousMode = _currentMode.ModeName;
            _currentMode = _playerModes[mode];
            _currentMode.Enter();
        }
        
        public PlayerMode GetPreviousMode()
        {
            return _previousMode;
        }
        
        public PlayerMode GetCurrentModeName()
        {
            return _currentMode.ModeName;
        }
        
        public void SetReplayingInputQueue(Queue<EntryState> queue)
        {
            ((ReplayingMode)_playerModes[PlayerMode.Replaying]).ReplayingInputQueue = queue;
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
            _playerModes.Add(PlayerMode.AI, new AIMode(character));
        }
    }
}