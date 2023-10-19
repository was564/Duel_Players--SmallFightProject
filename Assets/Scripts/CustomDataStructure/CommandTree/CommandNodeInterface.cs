using System.Collections.Generic;

namespace CustomDataStructure
{
    public interface CommandNodeInterface
    {
        public void JudgeCommand(
            LinkedList<CommandProcessor.InputAndTimeTuple> input,
            ref LinkedListNode<CommandProcessor.InputAndTimeTuple> node, 
            ref BehaviorEnumSet.Behavior result);

        public void AddCommand(List<BehaviorEnumSet.Button> command, BehaviorEnumSet.Behavior behavior);

        public BehaviorEnumSet.Button GetCondition();
        
        public BehaviorEnumSet.Behavior GetAction();
    }
}