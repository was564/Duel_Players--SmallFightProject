using System.Collections.Generic;

namespace CustomDataStructure
{
    public class CommandActionNode : CommandNodeInterface
    {
        public CommandActionNode(BehaviorEnumSet.Behavior state)
        {
            Action = state;
        }

        public BehaviorEnumSet.Behavior Action { get; private set; }
        
        public void AddCommand(List<BehaviorEnumSet.Button> command, BehaviorEnumSet.Behavior action)
        {
            return;
        }

        public void JudgeCommand(
            LinkedList<CommandProcessor.InputAndTimeTuple> input,
            ref LinkedListNode<CommandProcessor.InputAndTimeTuple> node,
            ref BehaviorEnumSet.Behavior result)
        {
            return;
        }

        public BehaviorEnumSet.Button GetCondition()
        {
            return BehaviorEnumSet.Button.Null;
        }

        public BehaviorEnumSet.Behavior GetAction()
        {
            return Action;
        }
    }
}