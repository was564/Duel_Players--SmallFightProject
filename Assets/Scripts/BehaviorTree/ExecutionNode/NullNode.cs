using System.Collections.Generic;

namespace BehaviorTree
{
    public class NullNode : StateExecutionNode
    {
        public NullNode(int nodeId, PlayerCharacter player)
            : base(nodeId, BehaviorEnumSet.State.Null, player, new List<BehaviorEnumSet.Button>())
        { }
    }
}