using System.Collections.Generic;
using UnityEngine.UI;

namespace CustomDataStructure
{
    public class CommandConditionNode : CommandNodeInterface
    {
        public CommandConditionNode(BehaviorEnumSet.Button condition, int depth)
        {
            Condition = condition;
            Depth = depth;
        }
        
        public int Depth { get; private set; }
        
        public BehaviorEnumSet.Button Condition { get; private set; }

        public List<CommandNodeInterface> Children { get; set; } = new List<CommandNodeInterface>();

        public void AddCommand(List<BehaviorEnumSet.Button> command, BehaviorEnumSet.Behavior action)
        {
            int commandDepth = command.Count - Depth - 1;
            if (commandDepth < 0)
            {
                Children.Add(new CommandActionNode(action));
                return;
            }
            
            BehaviorEnumSet.Button currentCondition = command[commandDepth];
            int nodeindexHascurrentCondition;
            for (nodeindexHascurrentCondition = 0; nodeindexHascurrentCondition < Children.Count ; nodeindexHascurrentCondition++)
            {
                if (currentCondition == Children[nodeindexHascurrentCondition].GetCondition())
                    break;
            }

            if (nodeindexHascurrentCondition == Children.Count)
            {
                CommandConditionNode childNode = new CommandConditionNode(currentCondition, Depth + 1);
                Children.Add(childNode);
                childNode.AddCommand(command, action);
            }
            else
            {
                Children[nodeindexHascurrentCondition].AddCommand(command, action);
            }
        }

        public void JudgeCommand(
            LinkedList<CommandProcessor.InputAndTimeTuple> input, 
            ref LinkedListNode<CommandProcessor.InputAndTimeTuple> node,
            ref BehaviorEnumSet.Behavior result)
        {
            BehaviorEnumSet.Button condition = this.GetCondition();
            if (Depth != 0 && condition == BehaviorEnumSet.Button.Null)
            {
                result = this.GetAction();
                return;
            }
            for (; node != null; node = node.Previous)
            {
                int index = 0;
                for (; index < Children.Count ; index++)
                {
                    if (Children[index].GetCondition() == node.Value.Input)
                    {
                        Children[index].JudgeCommand(input, ref node, ref result);
                    }
                }
            }
            

            return;
        }

        public BehaviorEnumSet.Button GetCondition()
        {
            return Condition;
        }

        public BehaviorEnumSet.Behavior GetAction()
        {
            return BehaviorEnumSet.Behavior.Null;
        }
    }
}