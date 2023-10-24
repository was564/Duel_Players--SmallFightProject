namespace CommandSkill
{
    public class InputConditionAndTimePair {
    
        public BehaviorEnumSet.InputSet Condition { get; private set; }
        public float Time { get; set; } = 0;
        
        public InputConditionAndTimePair(BehaviorEnumSet.InputSet button)
        {
            this.Condition = button;
        }
    }
}