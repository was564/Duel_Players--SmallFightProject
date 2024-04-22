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
        Grab,
        Guard,
        Assist,
        Size
    }

    public enum State
    {
        Null = -1,
        StandingIdle,
        Forward,
        Backward,
        StandingHit,
        CrouchHit,
        InAirHit,
        Jump,
        InAirIdle,
        Land,
        CrouchIdle,
        
        AttackStartIndex, // 10
        StandingPunch,
        StandingKick,
        CrouchPunch,
        CrouchKick,
        AiringPunch,
        AiringKick,
        StandingPunch236Skill,
        StandingKick236Skill,
        StandingPunch623Skill,
        StandingKick623Skill,
        StandingPunch6246SpecialSkillEnter,
        StandingPunch6246SpecialSkillAttack,
        AttackEndIndex, // 23
        
        GrabStart,
        GrabWait,
        BeCaught,
        GrabEscape,
        GrabAttack,
        
        DashOnGround,
        BackStepOnGroundState,
        
        GuardStartIndex,
        StandingGuard,
        CrouchGuard,
        AirGuard,
        GuardEndIndex,
        
        StandingStopHit,
        FallDown,
        GetUp,
        IntroPose,
        OutroPose,
        Size
    }

    /* Instead of using AttackName, use BehaviorState
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
    */

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
        Guard,
        Hit,
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
        Grab,
        StandingPunch236Skill,
        StandingKick236Skill,
        StandingPunch623Skill,
        StandingKick623Skill,
        StandingPunch6246SpecialSkill,
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