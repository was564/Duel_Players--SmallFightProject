using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterFSM.KohakuState.SkillState
{
    public class StandingPunch6246SpecialSkillEnterState : SkillStateInterface
    {
        private PlayerCharacter _enemyCharacterScript;
        
        public StandingPunch6246SpecialSkillEnterState(GameObject characterRoot)
            : base(BehaviorEnumSet.State.StandingPunch6246SpecialSkillEnter, characterRoot,
                BehaviorEnumSet.AttackLevel.SpecialMove, PassiveStateEnumSet.CharacterPositionState.InAir)
        {
            _enemyCharacterScript = PlayerCharacter.EnemyObject.GetComponent<PlayerCharacter>();
            
            MoveCommand = new List<BehaviorEnumSet.InputSet>()
            {
                BehaviorEnumSet.InputSet.Forward,
                BehaviorEnumSet.InputSet.Down,
                BehaviorEnumSet.InputSet.Backward,
                BehaviorEnumSet.InputSet.Forward
            };
            AttackTrigger = BehaviorEnumSet.Behavior.Punch;
            AvailableCommandPositionCondition.Add(PassiveStateEnumSet.CharacterPositionState.Crouch);
            AvailableCommandPositionCondition.Add(PassiveStateEnumSet.CharacterPositionState.OnGround);
            CommandManager.AddCommand(
                MoveCommand,
                AttackTrigger,
                AvailableCommandPositionCondition,
                BehaviorEnumSet.Behavior.StandingPunch6246SpecialSkill, (BehaviorEnumSet.AttackLevel)AttackLevel);
        }

        public override List<BehaviorEnumSet.InputSet> MoveCommand { get; protected set; }
        public override BehaviorEnumSet.Behavior AttackTrigger { get; protected set; }

        public override List<PassiveStateEnumSet.CharacterPositionState> AvailableCommandPositionCondition
        {
            get;
            protected set;
        }
            = new List<PassiveStateEnumSet.CharacterPositionState>();

        private float _speed = 2f;
        private float _radius = 0.7f;
        private float _degree;

        public override void Enter()
        {
            _degree = 0f;
            _isBackCamera = false;
            
            if(PlayerCharacter.transform.forward.x < 0) _isRight = false;
            else _isRight = true;
            
            CharacterJudgeBoxController.DisableHitBox();

            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            PlayerCharacter.RigidBody.velocity = Vector3.zero;

            CharacterAnimator.PlayAnimation("Standing6246PunchSpecialSkillCinema", CharacterAnimator.Layer.UpperLayer,
                true);
            CharacterAnimator.PlayAnimation("Standing6246PunchSpecialSkillCinema", CharacterAnimator.Layer.LowerLayer,
                true);

            CameraManager.SetCameraState(CameraManager.CameraState.FreeMoving);
            _enemyCharacterScript.SetModelObjectsVisible(false);
            FrameManager.PauseOtherCharactersInFrame(600, PlayerCharacter.gameObject.GetInstanceID());
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        private bool _isRight;
        private bool _isBackCamera = false;
        public override BehaviorEnumSet.State UpdateState()
        {
            Vector3 position = PlayerCharacter.transform.position;
            position.y = 0;
            PlayerCharacter.transform.position = position;
            
            float y = 1.2f;
            if (!_isBackCamera)
            {
                float x = _radius * Mathf.Cos(_degree * Mathf.Deg2Rad);
                float z = _radius * Mathf.Sin(_degree * Mathf.Deg2Rad);
                if (!_isRight) x *= -1; 
                
                CameraManager.SetGoalPoint(PlayerCharacter.transform.position + new Vector3(x, y, z));
                CameraManager.SetTargetPoint(PlayerCharacter.transform.position + Vector3.up * y);

                _degree += _speed * Time.deltaTime;
            }
            if (!_isBackCamera 
                && CharacterAnimator.GetCurrentAnimationDuration(CharacterAnimator.Layer.UpperLayer) >= 0.8f)
            {
                _isBackCamera = true;
                _enemyCharacterScript.SetModelObjectsVisible(true);
                CameraManager.SetGoalPoint(CameraManager.GetNormalCameraPoint());
            }

            if (_isBackCamera)
            {
                Vector3 targetPoint = 
                    Vector3.Lerp(PlayerCharacter.transform.position + Vector3.up * y, 
                        CameraManager.GetNormalCameraPoint() + Vector3.forward,
                        (CharacterAnimator.GetCurrentAnimationDuration(CharacterAnimator.Layer.UpperLayer) - 0.8f) * 5f);
                CameraManager.SetTargetPoint(targetPoint);
            }
            
            if (CharacterAnimator.IsEndCurrentAnimation("Standing6246PunchSpecialSkillCinema",
                    CharacterAnimator.Layer.UpperLayer))
                return BehaviorEnumSet.State.StandingPunch6246SpecialSkillAttack;
            else return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            CameraManager.SetCameraState(CameraManager.CameraState.Normal);
            _enemyCharacterScript.SetModelObjectsVisible(true);
            FrameManager.ResumeAllCharacters();
        }

    }
}   