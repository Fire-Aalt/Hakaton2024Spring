using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerFXController : MonoBehaviour
    {
        public SurfaceManager SurfaceManager { get; private set; }

        public bool IsFootstepParticleEmitting { get => SurfaceManager.ParticleSystems.footstepParticle.isEmitting; }
        public bool IsWallSlideParticleEmitting { get => SurfaceManager.ParticleSystems.wallSlideParticle.isEmitting; }

        private void Awake()
        {
            SurfaceManager = GetComponent<SurfaceManager>();
        }

        public void FootstepSFX()
        {
            PlaySound(SurfaceManager.SurfaceDetails.footstepSFX);
        }

        public void FootstepVFX(FootstepType type, bool enable)
        {
            if (enable)
            {
                var emission = SurfaceManager.ParticleSystems.footstepParticle.emission;

                switch (type)
                {
                    case FootstepType.Run:
                        emission.rateOverTime = SurfaceManager.Parameters.footstepEmissionWhileRun;
                        break;
                    case FootstepType.DodgeRoll:
                        emission.rateOverTime = SurfaceManager.Parameters.footstepEmissionWhileDodgeRoll;
                        break;
                }

                SurfaceManager.ParticleSystems.footstepParticle.Play();
            }
            else
                SurfaceManager.ParticleSystems.footstepParticle.Stop();
        }

        public void WallSlideVFX(bool enable)
        {
            if (enable)
                SurfaceManager.ParticleSystems.wallSlideParticle.Play();
            else
                SurfaceManager.ParticleSystems.wallSlideParticle.Stop();
        }

        public void JumpFX(JumpType type)
        {
            Transform transform = SurfaceManager.ParticleSystems.jumpParticle.transform;

            switch (type)
            {
                case JumpType.Jump:
                    transform.localPosition = new Vector2(transform.localPosition.x, SurfaceManager.Parameters.jumpOffsetWhileJump);
                    break;
                case JumpType.WallJump:
                    transform.localPosition = new Vector2(transform.localPosition.x, SurfaceManager.Parameters.jumpOffsetWhileWallJump);
                    break;
            }

            SurfaceManager.ParticleSystems.jumpParticle.Play();
            PlaySound(SurfaceManager.SurfaceDetails.jumpSFX);
        }

        public void LandFX(bool hardLand)
        {
            if (hardLand)
            {
                SurfaceManager.ParticleSystems.hardLandParticle.Play();
            }
            else
            {
                SurfaceManager.ParticleSystems.landParticle.Play();
                PlaySound(SurfaceManager.SurfaceDetails.landSFX);
            }
        }

        private void PlaySound(SoundEffectSO soundFX)
        {
            soundFX.Play(SoundEffectSO.SoundClipTrack.Sfx, transform);
        }

        public enum FootstepType
        {
            Run,
            DodgeRoll
        }

        public enum JumpType
        {
            Jump,
            WallJump
        }
    }
}