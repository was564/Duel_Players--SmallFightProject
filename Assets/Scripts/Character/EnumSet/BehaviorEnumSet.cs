public class BehaviorEnumSet
{
    public enum Button
    {
        Null = -1,
        Punch,
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
        Null = -1,
        StandingIdle,
        StandingPunch,
        StandingKick,
        Forward,
        Backward,
        StandingHit,
        Jump,
        InAirIdle,
        Land,
        CrouchIdle,
        CrouchPunch,
        CrouchKick,
        AiringPunch,
        AiringKick,
        StandingPunchSkill,
        
        Size
    }

    public enum AttackName
    {
        Punch = 0,
        Size
    }

    public enum AttackLevel
    {
        Move = 0,
        BasicAttack,
        Technique,
        SpecialMove,
        Size
    }

    // Include AttackName
    public enum Behavior
    {
        Null = -1,
        Punch,
        StandingPunchSkill,
        Kick,
        Forward,
        Backward,
        Stop,
        Jump,
        Crouch,
        Stand,
        
        Size
    }
}