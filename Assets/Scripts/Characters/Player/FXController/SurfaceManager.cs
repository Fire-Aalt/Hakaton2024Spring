using Game.CoreSystem;
using RenderDream.GameEssentials;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class SurfaceManager : Singleton<SurfaceManager>
    {
        [Header("Data")]
        [SerializeField] private PlayerFXDataSO _fXData;

        [Title("Default Particle Systems")]
        [SerializeField] private SurfaceParticleSystems _defaultParticleSystems;
        [SerializeField] private SurfaceParticleParams _defaultParameters;

        [Title("Overrides")]
        [SerializeField] private List<SurfaceParticleOverride> particleOverrides = new();

        public SurfaceType SurfaceType { get; private set; }
        public SurfaceDetails SurfaceDetails { get; private set; }
        public SurfaceParticleSystems ParticleSystems { get; private set; }
        public SurfaceParticleParams Parameters { get; private set; }

        public CoreComp<CollisionSenses> CollisionSenses { get; private set; }
        public Core Core { get; private set; }

        public Dictionary<string, SurfaceType> SurfaceTagsDictionary { get; private set; }
        public string[] SurfaceTagsNames { get; private set; }

        private Vector2 _groundCheckSize;
        private Vector2 _wallCheckSize;
        private LayerMask _groundLayer;
        private Transform _groundCheck;
        private Transform _wallCheck;

        private bool _defaultParticles;
        private Collider2D[] _results;

        private void Start()
        {
            Core = GetComponentInParent<Player>().Core;
            CollisionSenses = new CoreComp<CollisionSenses>(Core);

            _groundCheckSize = CollisionSenses.Comp.GroundCheckSize;
            _wallCheckSize = CollisionSenses.Comp.WallCheckSize;
            _groundLayer = CollisionSenses.Comp.WhatIsGround;

            _groundCheck = CollisionSenses.Comp.GroundCheck;
            _wallCheck = CollisionSenses.Comp.WallCheck;

            SurfaceTagsDictionary = new Dictionary<string, SurfaceType>
            {
                { "GrassSurface", SurfaceType.Grass },
                { "RockSurface", SurfaceType.Rock },
                { "MetalSurface", SurfaceType.Metal },
                { "SandSurface", SurfaceType.Sand }
            };
            SurfaceTagsNames = SurfaceTagsDictionary.Keys.ToArray();

            _results = new Collider2D[8];
        }

        private void FixedUpdate()
        {
            Collider2D groundCollider = OverlapBoxNonAlloc(_groundCheck.position, _groundCheckSize, _groundLayer);
            Collider2D wallCollider = null;

            if (groundCollider == null)
            {
                wallCollider = OverlapBoxNonAlloc(_wallCheck.position, _wallCheckSize, _groundLayer);
            }

            SurfaceType newSurfaceType = SurfaceType.None;
            if (groundCollider != null)
            {
                newSurfaceType = GetSurfaceTypeNonAlloc(groundCollider);
            }
            else if (wallCollider != null)
            {
                newSurfaceType = GetSurfaceTypeNonAlloc(wallCollider);
            }

            if (newSurfaceType != SurfaceType.None && newSurfaceType != SurfaceType)
            {
                SurfaceParticleSystems previousParticleSystems = ParticleSystems;
                SurfaceType = newSurfaceType;
                SurfaceDetails = _fXData.surfaceDictionary[SurfaceType];

                if (TryChangeParticles(SurfaceType))
                {
                    StopParticles(previousParticleSystems);
                }
            }
        }

        public SurfaceType GetSurfaceTypeNonAlloc(Collider2D collider)
        {
            foreach (var tag in SurfaceTagsNames)
            {
                if (collider.CompareTag(tag))
                {
                    return SurfaceTagsDictionary[tag];
                }
            }
            return SurfaceType.None;
        }

        private Collider2D OverlapBoxNonAlloc(Vector2 position, Vector2 size, LayerMask layerMask)
        {
            int count = Physics2D.OverlapBoxNonAlloc(position, size, 0f, _results, layerMask);
            return count > 0 ? _results[0] : null;
        }

        private bool TryChangeParticles(SurfaceType newSurfaceType)
        {
            bool found = false;

            foreach (var particleOverride in particleOverrides)
            {
                if (particleOverride.surfaceType == newSurfaceType)
                {
                    ParticleSystems = particleOverride.particleSystems;
                    Parameters = particleOverride.parameters;

                    _defaultParticles = false;
                    found = true;
                    break;
                }
            }

            if (!found && !_defaultParticles)
            {
                _defaultParticles = true;
                found = true;

                ParticleSystems = _defaultParticleSystems;
                Parameters = _defaultParameters;
            }

            return found;
        }

        private void StopParticles(SurfaceParticleSystems previousParticleSystems)
        {
            if (previousParticleSystems.footstepParticle != null)
            {
                if (previousParticleSystems.footstepParticle.isEmitting)
                {
                    previousParticleSystems.footstepParticle.Stop();
                    ParticleSystems.footstepParticle.Play();
                }
            }
            if (previousParticleSystems.wallSlideParticle != null)
            {
                if (previousParticleSystems.wallSlideParticle.isEmitting)
                {
                    previousParticleSystems.wallSlideParticle.Stop();
                    ParticleSystems.wallSlideParticle.Play();
                }
            }
        }
    }

    public enum SurfaceType
    {
        None,
        Grass,
        Rock,
        Metal,
        Sand
    }

    [Serializable]
    public struct SurfaceParticleOverride
    {
        [Header("Key")]
        public SurfaceType surfaceType;

        [Header("Value")]
        public SurfaceParticleSystems particleSystems;
        public SurfaceParticleParams parameters;
    }

    [Serializable]
    public struct SurfaceParticleSystems
    {
        [Header("Particle Systems")]
        public ParticleSystem footstepParticle;
        public ParticleSystem wallSlideParticle;
        public ParticleSystem jumpParticle;
        public ParticleSystem landParticle;
        public ParticleSystem hardLandParticle;
    }

    [Serializable]
    public struct SurfaceParticleParams
    {
        [Header("Parameters")]
        public int footstepEmissionWhileRun;
        public int footstepEmissionWhileDodgeRoll;
        [Space]
        public float jumpOffsetWhileJump;
        public float jumpOffsetWhileWallJump;
    }
}
