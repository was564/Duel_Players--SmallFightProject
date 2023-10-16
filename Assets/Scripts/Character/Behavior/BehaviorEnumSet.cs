public class BehaviorEnumSet
{
    public enum Button
    {
        Idle = 0,
        Punch,
        Kick,
        Forward,
        Backward,
        Jump,
        Crouch,
        Guard,
        Assist,
        Size
    }

    public enum State
    {
        StandingIdle = 0,
        StandingPunch,
        Forward,
        Backward,
        StandingHit,
        Jump,
        InAirIdle,
        Land,
        CrouchIdle,
        Size
    }

    public enum AttackName
    {
        Punch = 0,
        Size
    }

    // Include AttackName
    public enum Behavior
    {
        Idle = 0,
        Punch,
        Forward,
        Backward,
        Jump,
        Crouch,
        
        Size
    }
}