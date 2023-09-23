using UnityEngine;

namespace Character.CharacterPassive
{
    public abstract class PassiveStateInterface
    {
        protected PassiveStateInterface(GameObject characterRoot)
        {
            this.MyCharacter = characterRoot;
        }
        
        protected GameObject MyCharacter;
        public float RemainTime { get; protected set; }

        public abstract void EnterPassiveState();

        public abstract void QuitPassiveState();
    }
}

public enum PassiveState
{
    LightWeight,
    Size
}