namespace BehaviorTree
{
    public class WaitNode : ExecutionNode
    {
        private int _waitFrameTime;

        private int _currentFrameTime = 0;

        public WaitNode(int nodeId, PlayerCharacter player, int waitFrameTime) : base(nodeId, player)
        {
            _waitFrameTime = waitFrameTime;
        }
        
        public override ResultNode Evaluate()
        {
            switch (CurrentResult)
            {
                case ResultNode.Success:
                    return ResultNode.Success;
                case ResultNode.Failure:
                    return ResultNode.Failure;
                case ResultNode.Running:
                    break;
                default:
                    _currentFrameTime = 0;
                    break;
            }
            
            if (_currentFrameTime < _waitFrameTime)
            {
                _currentFrameTime++;
                SetResult(ResultNode.Running);
                return ResultNode.Running;
            }
            else
            {
                _currentFrameTime = 0;
                SetResult(ResultNode.Success);
                return ResultNode.Success;
            }
        }
    }
}