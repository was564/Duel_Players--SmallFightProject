using UnityEngine;

namespace Character.CharacterPassive
{
    public abstract class PassiveStateInterface : MonoBehaviour
    {
        protected CharacterStructure _character;
        public float RemainTime { get; protected set; }
        
        private void Start()
        {
            _character = this.GetComponent<CharacterStructure>();
        }

        public abstract void EffectPassive();
    }
}