using System.Collections.Generic;

namespace BehaviorTree
{
    public class BackwardWalkingNode : StateExecutionNode
    {
        public BackwardWalkingNode(int nodeId, PlayerCharacter player)
            : base(nodeId, BehaviorEnumSet.State.Backward, player,
                new List<BehaviorEnumSet.Button>()
                {
                    BehaviorEnumSet.Button.Backward
                }
            )
        { }
        
        
    }
}