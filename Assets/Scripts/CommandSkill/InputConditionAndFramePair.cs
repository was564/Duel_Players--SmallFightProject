namespace CommandSkill
{
    public class InputConditionAndFramePair {
    
        public BehaviorEnumSet.InputSet Condition { get; private set; }
        public int Frame { get; set; } = 0;
        
        public InputConditionAndFramePair(BehaviorEnumSet.InputSet button)
        {
            this.Condition = button;
        }
    }
}