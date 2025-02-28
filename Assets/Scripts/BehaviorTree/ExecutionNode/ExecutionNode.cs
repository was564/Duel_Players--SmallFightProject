namespace BehaviorTree
{
    public abstract class ExecutionNode : Node
    {
        protected PlayerCharacter _playerCharacter;
        public ExecutionNode(int nodeId, PlayerCharacter player) : base(nodeId)
        {
            _playerCharacter = player;
        }
    }
}