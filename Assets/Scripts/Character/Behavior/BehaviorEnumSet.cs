public class BehaviorEnumSet
{
    public enum Button
    {
        Punch = 0,
        Kick,
        Forward,
        Backward,
        Stop,
        Jump,
        Stand,
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
        Null = -1,
        Punch,
        Forward,
        Backward,
        Stop,
        Jump,
        Crouch,
        Stand,
        
        Size
    }
}