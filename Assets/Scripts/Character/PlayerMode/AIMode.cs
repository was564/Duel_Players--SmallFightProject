using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Unity.VisualScripting;
using UnityEngine;

namespace Character.PlayerMode
{
    public class AIMode : PlayerModeInterface
    {
        private BehaviorTreeManager _behaviorTreeManager;

        // coroutine stop issue
        // https://discussions.unity.com/t/stopcoroutine-is-not-stopping-my-coroutines/134523/2
        private Stack<Coroutine> _coroutineStack = new Stack<Coroutine>();
        private int _coroutineStopTime = -60;
        
        public AIMode(PlayerCharacter character)
            : base(PlayerModeManager.PlayerMode.AI, character)
        {
            _behaviorTreeManager = new BehaviorTreeManager(character);
        }

        public override void Enter()
        {
            if(_coroutineStack.Count == 0)
                _coroutineStack.Push(Character.StartCoroutine(CoroutineUpdate()));
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
            if (_coroutineStack.Count > 0)
            {
                Character.StopCoroutine(_coroutineStack.Peek());
                _coroutineStack.Pop();
            }
            _coroutineStopTime = FrameManager.CurrentFrame;
        }
        
        private IEnumerator CoroutineUpdate()
        {
            // When Starting
            int coroutineBetweenStartAndStopTime = FrameManager.CurrentFrame - _coroutineStopTime;
            if (coroutineBetweenStartAndStopTime < 60) 
                // 0.016667f is 1/60, 0.1f is border time
                yield return new WaitForSeconds((float)coroutineBetweenStartAndStopTime * 0.016667f + 0.1f);
            if (_coroutineStack.Count >= 2)
            {
                if(_coroutineStack.Count > 0)
                    _coroutineStack.Pop();
            
                yield return null;
            }
                
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
            
            yield return null;
        }
    }
}