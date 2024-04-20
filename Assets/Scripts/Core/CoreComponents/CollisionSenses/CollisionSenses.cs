using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.CoreSystem
{
    public class CollisionSenses : CoreComponent
    {
        [Title("Checks")]
        [Tooltip("This collider will be used to create Ground Check Area")]
        [SerializeField] private BoxCollider2D _collider;
        public BoxCollider2D MovementCollider { get; private set; }

        public Transform GroundCheck 
        {
            get => groundCheck;
            private set => groundCheck = value;
        }
        public Transform WallCheck
        {
            get => wallCheck;
            private set => wallCheck = value;
        }
        public Transform WallLedgeCheck
        {
            get => wallLedgeCheck;
            private set => wallLedgeCheck = value;
        }
        public Transform LedgeCheckHorizontal {
            get => ledgeCheckHorizontal;
            private set => ledgeCheckHorizontal = value;
        }
        public Transform LedgeCheckVertical {
            get => ledgeCheckVertical;
            private set => ledgeCheckVertical = value;
        }
        public Transform CeilingCheck {
            get => ceilingCheck;
            private set => ceilingCheck = value;
        }

        public Vector2 GroundCheckSize { get => groundCheckSize; set => groundCheckSize = value; }
        public Vector2 WallCheckSize { get => wallCheckSize; set => wallCheckSize = value; }
        public float WallLedgeCheckDistance { get => wallLedgeCheckDistance; set => wallLedgeCheckDistance = value; }
        public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }

        private Vector2 workspace;

        [Header("Overlap Box")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private Transform ceilingCheck;
        [Header("Raycast")]
        [SerializeField] private Transform wallLedgeCheck;
        [SerializeField] private Transform ledgeCheckHorizontal;
        [SerializeField] private Transform ledgeCheckVertical;
        [Header("Properties")]
        [SerializeField] private Vector2 groundCheckSize;
        [SerializeField] private Vector2 wallCheckSize;
        [SerializeField] private Vector2 ceilingCheckSize;
        [SerializeField] private float wallLedgeCheckDistance;

        [SerializeField] private LayerMask whatIsGround;

        private Movement Movement { get => movement ??= core.GetCoreComponent<Movement>(); }
        private Movement movement;

        private Collider2D[] _results;

        protected override void Awake()
        {
            base.Awake();

            if (_collider != null)
                MovementCollider = _collider;
            else
                MovementCollider = GetComponentInParent<BoxCollider2D>();

            _results = new Collider2D[10];
        }

        public bool Ground
        {
            get => OverlapBox(GroundCheck.position, groundCheckSize, whatIsGround);
        }

        public bool Ceiling
        {
            get => OverlapBox(CeilingCheck.position, ceilingCheckSize, whatIsGround);
        }

        public bool Wall
        {
            get => OverlapBox(WallCheck.position, wallCheckSize, whatIsGround);
        }

        public bool WallLedge
        {
            get => Physics2D.Raycast(WallLedgeCheck.position, Vector2.right * Movement.FacingDirection, wallLedgeCheckDistance, whatIsGround);
        }

        public bool LedgeHorizontal
        {
            get => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * Movement.FacingDirection, wallLedgeCheckDistance, whatIsGround);
        }

        public bool LedgeVertical
        {
            get => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallLedgeCheckDistance, whatIsGround);
        }


        private bool OverlapBox(Vector2 point, Vector2 size, int layerMask)
        {
            int count = Physics2D.OverlapBoxNonAlloc(point, size, 0f, _results, layerMask);
            return count > 0;
        }

        public void SetColliderHeight(float height)
        {
            Vector2 center = MovementCollider.offset;
            workspace.Set(MovementCollider.size.x, height);

            center.y += (height - MovementCollider.size.y) / 2;

            MovementCollider.size = workspace;
            MovementCollider.offset = center;
        }

        private void OnDrawGizmos()
        {
            if (wallCheck != null)
                Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);

            if (groundCheck != null)
                Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}