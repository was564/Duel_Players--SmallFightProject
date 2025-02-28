namespace BehaviorTree
{
    public class WaitUntilIfTrueNode : ExecutionNode
    {
        private System.Func<bool> _condition;
        
        private readonly int _waitLimitFrameTime;
        private int _currentFrameTime = 0;
        
        public WaitUntilIfTrueNode(int nodeId, PlayerCharacter player, System.Func<bool> condition, int waitLimitFrameTime=120) : base(nodeId, player)
        {
            _waitLimitFrameTime = waitLimitFrameTime;
            _condition = condition;
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
            
            if (_condition())
            {
                SetResult(ResultNode.Success);
                return ResultNode.Success;
            }
            else if(_currentFrameTime >= _waitLimitFrameTime)
            {
                SetResult(ResultNode.Failure);
                return ResultNode.Failure;
            }
            else
            {
                _currentFrameTime++;
                SetResult(ResultNode.Running);
                return ResultNode.Running;
            }
        }
        
    }
}