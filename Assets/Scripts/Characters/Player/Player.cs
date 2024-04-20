using UnityEngine;
using Game.CoreSystem;
using Sirenix.OdinInspector;

namespace Game
{
    public class Player : MonoBehaviour
    {
        private static Player _player;
        public static Player Current
        {
            get
            {
                if (_player == null)
                    _player = FindObjectOfType<Player>();
                return _player;
            }
        }

        [Title("Data")]
        public PlayerDataSO PlayerData;
        public PlayerMovementDataSO MovementData;

        [Title("References")]
        public SpriteRenderer SpriteRenderer;
        public Animator Animator;
        public Rigidbody2D Rigidbody;
        public BoxCollider2D Collider;
        public PlayerInputHandler InputHandler;

        #region State Datas
        [Title("Base States")]
        public PlayerIdleData IdleStateData;
        public PlayerRunData RunStateData;
        public PlayerLandData LandStateData;
        public PlayerInAirData InAirStateData;
        public PlayerJumpData JumpStateData;

        [Header("Conditional States")]
        public PlayerHitData HitStateData;
        public PlayerDeathData DeathStateData;

        [Header("Ability States")]
        public PlayerDoubleJumpData DoubleJumpStateData;
        #endregion

        public PlayerStateMachine StateMachine { get; private set; }

        #region States
        public PlayerIdleState IdleState { get; private set; }
        public PlayerRunState RunState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerLandState LandState { get; private set; }
        public PlayerInAirState InAirState { get; private set; }
        public PlayerHitState HitState { get; private set; }
        public PlayerDeathState DeathState { get; private set; }
        public PlayerDoubleJumpState DoubleJumpState { get; private set; }
        #endregion

        // Components
        public Core Core { get; private set; }
        public PlayerAbilityInventory AbilityInventory { get; private set; }
        public CollectibleManager CollectibleManager { get; private set; }
        public PlayerFXController FXController { get; private set; }

        private PlayerMovement _movement;

        private void Awake()
        {
            Core = GetComponentInChildren<Core>();

            StateMachine = new PlayerStateMachine();

            IdleState = new PlayerIdleState(this, IdleStateData);
            RunState = new PlayerRunState(this, RunStateData);
            LandState = new PlayerLandState(this, LandStateData);
            InAirState = new PlayerInAirState(this, InAirStateData);
            JumpState = new PlayerJumpState(this, JumpStateData);
            DoubleJumpState = new PlayerDoubleJumpState(this, DoubleJumpStateData);
            HitState = new PlayerHitState(this, HitStateData);
            DeathState = new PlayerDeathState(this, DeathStateData);
        }

        private void Start()
        {
            AbilityInventory = GetComponentInChildren<PlayerAbilityInventory>();
            CollectibleManager = GetComponentInChildren<CollectibleManager>();
            FXController = GetComponentInChildren<PlayerFXController>();

            _movement = Core.GetCoreComponent<PlayerMovement>();
            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            Core.LogicUpdate();
            StateMachine.CurrentState?.LogicUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState?.PhysicsUpdate();
        }

        private void OnDrawGizmos()
        {
            StateMachine?.CurrentState?.DrawGizmos();
        }

        public void AnimationTrigger(string key) => StateMachine.CurrentState.AnimationTrigger(key);
        public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    }
}