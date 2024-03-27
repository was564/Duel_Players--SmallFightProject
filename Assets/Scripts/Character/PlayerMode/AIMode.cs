using System.Collections;
using BehaviorTree;
using Unity.VisualScripting;
using UnityEngine;

namespace Character.PlayerMode
{
    public class AIMode : PlayerModeInterface
    {
        private BehaviorTreeManager _behaviorTreeManager;

        public AIMode(PlayerCharacter character)
            : base(PlayerModeManager.PlayerMode.AI, character)
        {
            _behaviorTreeManager = new BehaviorTreeManager(character);
        }
        
        public override void Update()
        {
            Character.StartCoroutine(CoroutineUpdate());
            Character.DecideBehaviorByInput();
            Character.UpdatePassiveState();
            Character.StateManager.UpdateState();
            Character.ComboManagerInstance.UpdateComboManager();
        }

        private IEnumerator CoroutineUpdate()
        {
            Node.ResultNode result = _behaviorTreeManager.BehaviorTreeUpdate();
            Debug.Log(Character.name + result);
            switch (result)
            {
                case Node.ResultNode.Running:
                    yield return new WaitForNextFrameUnit();
                    break;
                case Node.ResultNode.Success:
                    yield return new WaitForSeconds(1.0f);
                    break;
                case Node.ResultNode.Failure:
                    yield return null;
                    break;
            }
        }
    }
}