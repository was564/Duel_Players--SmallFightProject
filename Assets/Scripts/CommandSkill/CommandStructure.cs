using System.Collections.Generic;

namespace CommandSkill
{
    // struct는 데이터가 스택에 할당되기 때문에 더 빠르다.
    // Reference : https://mdfarragher.medium.com/whats-faster-in-c-a-struct-or-a-class-99e4761a7b76
    // 해당 구조체 속도는 모르겠음..
    // 추후 개선할 수도 있는 사항은 time대신 Frame수를 이용해보기 (float -> int)
    // Generic + Class WritablePair 고려하기 상담
    
    // 지금까지 들어온 입력을 보고 매 프레임마다 커맨드인지 검사하려다가 실시간성을 잘 이용하지 못한 느낌이 들어 변경
    public class CommandStructure
    {
        public List<InputConditionAndTimePair> Command { get; private set; } = new List<InputConditionAndTimePair>();
        public int Depth { get; set; } = 0;

        public BehaviorEnumSet.Behavior AttackTrigger { get; private set; }

        public float InputStartingTime { get; set; }

        public BehaviorEnumSet.Behavior CommandBehavior { get; private set; }

        public List<PassiveStateEnumSet.CharacterPositionState> AvailableCommandPositionState { get; private set; }
        
        public CommandStructure(
            List<BehaviorEnumSet.InputSet> command,
            BehaviorEnumSet.Behavior attackTrigger,
            List<PassiveStateEnumSet.CharacterPositionState> availableCommandPositionState,
            BehaviorEnumSet.Behavior result)
        {
            AttackTrigger = attackTrigger;
            CommandBehavior = result;
            AvailableCommandPositionState = availableCommandPositionState;
            foreach (var input in command)
            {
                this.Command.Add(new InputConditionAndTimePair(input));
            }
        }
    }
}