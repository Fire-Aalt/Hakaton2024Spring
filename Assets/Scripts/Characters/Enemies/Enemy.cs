using Game;
using Game.CoreSystem;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnVisible, OnHidden, OnDeath;

    [Title("Data")]
    [InlineEditor] public EnemyDataSO Data;
    public float speed;

    [field: SerializeField, Title("References")] public Animator Animator { get; private set; }
    [field: SerializeField] public Hitbox BodyHitbox { get; private set; }

    public Core Core { get; private set; }
    public Collider2D BodyCollider { get; private set; }

    private Movement _movement;
    private SpriteShaderController _shaderController;
    private EnemyStats _stats;

    private Player _player;
    private bool _started;

    private void Awake()
    {
        BodyCollider = GetComponent<Collider2D>();

        Core = GetComponentInChildren<Core>(true);
        _movement = Core.GetCoreComponent<Movement>();
        _shaderController = Core.GetCoreComponent<SpriteShaderController>();
        _stats = Core.GetCoreComponent<EnemyStats>();
    }

    private void Start()
    {
        _player = Player.Current;
        _stats.MaxHealth = Data.stats.health;
        _stats.Defense = Data.stats.defence;
        BodyHitbox.damageToPlayer = Data.stats.damage;
        _started = true;
    }

    private void SmartUpdate()
    {
        if (!_started) return;

        if (_stats.IsAlive)
        {
            var direction = _player.transform.position - transform.position;
            direction.Normalize();

            _movement.SetVelocity(speed, direction);
            _movement.CheckIfShouldFlip(direction.x);
        }
        else
        {
            _movement.SetVelocityZero();
        }
    }

    private void Death()
    {
        BodyCollider.enabled = false;
        OnDeath?.Invoke(this);
    }

    public void OnSpawn()
    {
        _stats.Revive(1f);

        BodyCollider.enabled = true;
        _shaderController.ResetEffects();
    }

    public void OnDespawn()
    {

    }

    private void HandleVisible() => OnVisible?.Invoke(this);
    private void HandleHidden() => OnHidden?.Invoke(this);

    private void OnEnable()
    {
        _stats.OnDeath += Death;
    }

    private void OnDisable()
    {
        _stats.OnDeath -= Death;
    }
}
