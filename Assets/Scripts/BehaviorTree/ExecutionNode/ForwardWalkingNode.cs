namespace BehaviorTree
{
    public class ForwardWalkingNode : StateExecutionNode
    {
        public ForwardWalkingNode(int nodeId, PlayerCharacter player)
            : base(nodeId, BehaviorEnumSet.State.Forward, player)
        { }
    }
}