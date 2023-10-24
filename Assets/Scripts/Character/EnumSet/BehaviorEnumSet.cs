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