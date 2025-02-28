public class PassiveStateEnumSet
{
    public enum CharacterPositionState
    {
        OnGround = 1,
        InAir,
        Crouch,
        
        Size
    }
    
    public enum PassiveState
    {
        LightWeight,
        StoppingOnGround,
        FrameStopping,
        
        Size
    }
}
