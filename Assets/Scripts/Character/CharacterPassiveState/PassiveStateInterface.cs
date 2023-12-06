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
        public int RemainFrame { get; set; } = 0;
        public bool IsActivate { get; set; } = false;

        public abstract void EnterPassiveState();

        public abstract void UpdatePassiveState();
        public abstract void QuitPassiveState();
    }
}

