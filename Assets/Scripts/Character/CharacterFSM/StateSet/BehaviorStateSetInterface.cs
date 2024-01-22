using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.CharacterFSM
{
    public abstract class BehaviorStateSetInterface
    {
        protected abstract List<BehaviorStateInterface> StateSet { get; set; }
        protected abstract GameObject CharacterRoot { get; set; }

        public BehaviorStateSetInterface(GameObject characterRoot)
        {
            CharacterRoot = characterRoot;
        }
        
        // for Init in Constructor
        protected void InitClass()
        {
            BehaviorStateInterface nullState = new NullState(CharacterRoot);
            StateSet = Enumerable.Repeat(nullState, (int)BehaviorEnumSet.State.Size).ToList();
        }
        
        protected void BindState(BehaviorEnumSet.State state, BehaviorStateInterface behaviorState)
        {
            StateSet[(int)state] = behaviorState;
        }
        
        public virtual BehaviorStateInterface GetStateInfo(BehaviorEnumSet.State state)
        {
            return StateSet[(int)state];
        }
    }
}