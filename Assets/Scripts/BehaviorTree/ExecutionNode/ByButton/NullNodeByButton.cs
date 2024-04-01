using System.Collections.Generic;

namespace BehaviorTree
{
    public class NullNodeByButton : StateExecutionNodeByButton
    {
        public NullNodeByButton(int nodeId, PlayerCharacter player)
            : base(nodeId, BehaviorEnumSet.State.Null, player, new List<BehaviorEnumSet.Button>())
        { }
    }
}