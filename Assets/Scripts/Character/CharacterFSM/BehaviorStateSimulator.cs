using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterFSM
{
    public class BehaviorStateSimulator
    {
        /*
        protected Dictionary<BehaviorEnumSet.State, BehaviorStateInterface> BehaviorStateSet
            = new Dictionary<BehaviorEnumSet.State, BehaviorStateInterface>();
        */
        public BehaviorStateSetInterface StateSet { get; private set; }
        protected GameObject RootCharacterObject;
        protected GameObject Wall;
        
        public BehaviorStateInterface CurrentState { get; protected set; }

        protected ComboManager ComboManagerInstance;
        
        public BehaviorStateSimulator(GameObject characterObject, GameObject wall, ComboManager comboManager)
        {
            RootCharacterObject = characterObject.transform.root.gameObject;
            ComboManagerInstance = comboManager;
            Wall = wall;
            
            //_currentState.Enter();
        }
        
        public void Initialize(BehaviorStateSetInterface stateSet)
        {
            StateSet = stateSet;
            CurrentState = StateSet.GetStateInfo(BehaviorEnumSet.State.StandingIdle);
        }
        
        public virtual void ChangeState(BehaviorEnumSet.State nextState)
        {
            if(nextState == BehaviorEnumSet.State.Null) return;
            CurrentState = StateSet.GetStateInfo(nextState);
        }
        
        public virtual void UpdateState() { }

        public virtual void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            BehaviorEnumSet.State resultState = ComboManagerInstance.TryGetActivateSkillState(this.CurrentState.StateName, behavior);
            if (resultState != BehaviorEnumSet.State.Null) 
                this.ChangeState(resultState);
            else
            {
                resultState = this.CurrentState.GetResultStateByHandleInput(behavior);
                this.ChangeState(resultState);
            }
        }
    }
}