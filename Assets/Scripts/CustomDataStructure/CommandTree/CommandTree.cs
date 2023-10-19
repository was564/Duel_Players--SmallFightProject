using System.Collections.Generic;

namespace CustomDataStructure
{
    public class CommandTree
    {
        public CommandTree()
        {
            Root = new CommandConditionNode(BehaviorEnumSet.Button.Null, 0);
        }
        
        public CommandConditionNode Root { get; set; }

        public Queue<BehaviorEnumSet.State> CommandQueue = new Queue<BehaviorEnumSet.State>();

        public void AddCommand(List<BehaviorEnumSet.Button> command, BehaviorEnumSet.Behavior action)
        {
            Root.AddCommand(command, action);
        }

        public BehaviorEnumSet.Behavior JudgeCommand(
            LinkedList<CommandProcessor.InputAndTimeTuple> input)
        {
            BehaviorEnumSet.Behavior result = BehaviorEnumSet.Behavior.Null;
            var lastNode = input.Last;
            Root.JudgeCommand(input, ref lastNode, ref result);

            if (result == BehaviorEnumSet.Behavior.Null)
                return BehaviorEnumSet.Behavior.Punch;
            
            
            return BehaviorEnumSet.Behavior.Punch;
        }
    }
    
}