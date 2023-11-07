using UnityEngine;

namespace Character.CharacterPassiveState
{
    public abstract class PassiveStateInterface
    {
        protected PassiveStateInterface(GameObject characterRoot)
        {
            this.MyCharacter = characterRoot;
        }
        
        protected GameObject MyCharacter;
        public float RemainTime { get; set; } = 0.0f;
        public bool IsActivate { get; set; } = false;

        public abstract void EnterPassiveState();

        public abstract void UpdatePassiveState();
        public abstract void QuitPassiveState();
    }
}

