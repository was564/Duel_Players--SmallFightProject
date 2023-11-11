public class BehaviorEnumSet
{
    public enum Button : int
    {
        Null = -1,
        Forward,
        Backward,
        Stop,
        Jump,
        Stand,
        Crouch,
        Punch,
        Kick,
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
        CrouchHit,
        InAirHit,
        Jump,
        InAirIdle,
        Land,
        CrouchIdle,
        CrouchPunch,
        CrouchKick,
        AiringPunch,
        AiringKick,
        StandingPunchSkill,
        DashOnGround,
        StandingGuard,
        CrouchGuard,
        AirGuard,
        FallDown,
        GetUp,
        Size
    }

    public enum AttackName
    {
        Punch = 0,
        Kick = 1,
        Size
    }

    public enum AttackLevel
    {
        Move = 0,
        BasicAttack,
        Technique,
        SpecialMove,
        
        Hit,
        Size
    }

    // Include AttackName
    public enum Behavior
    {
        Null = -1,
        Forward,
        Backward,
        Stop,
        Jump,
        Crouch,
        Stand,
        Guard,
        StandingPunchSkill,
        Dash,
        Punch,
        Kick,
        Size
    }
    
    public enum InputSet
    {
        Idle = 0,
        Backward = 1,
        Forward = 2,
        Down = 3,
        BackwardDown = 4,
        ForwardDown = 5,
        Up = 6,
        BackwardUp = 7,
        ForwardUp = 8,
        
        Size
    }
}