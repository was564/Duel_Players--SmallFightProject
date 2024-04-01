using System.Collections;
using BehaviorTree;
using Unity.VisualScripting;
using UnityEngine;

namespace Character.PlayerMode
{
    public class AIMode : PlayerModeInterface
    {
        private BehaviorTreeManager _behaviorTreeManager;

        // coroutine stop bug
        // https://discussions.unity.com/t/stopcoroutine-is-not-stopping-my-coroutines/134523/2
        private Coroutine _coroutineUpdate;
        private int _coroutineStopTime = -60;
        
        public AIMode(PlayerCharacter character)
            : base(PlayerModeManager.PlayerMode.AI, character)
        {
            _behaviorTreeManager = new BehaviorTreeManager(character);
        }

        public override void Enter()
        {
            _coroutineUpdate = Character.StartCoroutine(CoroutineUpdate());
        }

        public override void Update()
        {
            //Character.DecideBehaviorByInput();
            Character.UpdatePassiveState();
            Character.StateManager.UpdateState();
            Character.ComboManagerInstance.UpdateComboManager();
        }

        public override void Quit()
        {
            Character.StopCoroutine(_coroutineUpdate);
            _coroutineStopTime = FrameManager.CurrentFrame;
        }
        
        private IEnumerator CoroutineUpdate()
        {
            // When Starting
            int coroutineBetweenStartAndStopTime = FrameManager.CurrentFrame - _coroutineStopTime;
            if (coroutineBetweenStartAndStopTime < 60) yield return new WaitForSeconds((float)coroutineBetweenStartAndStopTime/60.0f);
            
            
            while (true)
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
}