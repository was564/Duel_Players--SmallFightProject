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
        StandingPunch236Skill,
        StandingKick236Skill,
        StandingPunch623Skill,
        StandingKick623Skill,
        DashOnGround,
        BackStepOnGroundState,
        StandingGuard,
        CrouchGuard,
        AirGuard,
        FallDown,
        GetUp,
        IntroPose,
        OutroPose,
        Size
    }

    public enum AttackName
    {
        StandingPunch = 0,
        StandingKick = 1,
        CrouchPunch = 2,
        CrouchKick = 3,
        AiringPunch = 4,
        AiringKick = 5,
        StandingPunch236Skill = 6,
        StandingKick236Skill = 7,
        StandingPunch623Skill = 8,
        StandingKick623Skill = 9,
        Size
    }

    public enum AttackPosition
    {
        Air = 0,
        Stand,
        Crouch,
        Size
    }

    public enum AttackLevel
    {
        Move = 0,
        CancelableMove,
        BasicAttack,
        Technique,
        SpecialMove,
        Hit,
        Guard,
        Size
    }

    public enum HitReactLevel
    {
        HitInPlace = 0,
        HitFlyOut,
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
        StandingPunch236Skill,
        StandingKick236Skill,
        StandingPunch623Skill,
        StandingKick623Skill,
        Dash,
        BackStep,
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