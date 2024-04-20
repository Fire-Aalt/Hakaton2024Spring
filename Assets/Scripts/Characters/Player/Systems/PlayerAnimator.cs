using UnityEngine;

namespace Game
{
    public class PlayerAnimator : MonoBehaviour
    {
        public static string FINISH_STATE_KEY = "finishState";

        private Player _player;

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
        }

        private void AnimationTrigger(string key) => _player.AnimationTrigger(key);
        private void AnimationStateFinishTrigger() => _player.AnimationTrigger(FINISH_STATE_KEY);
        private void AnimationFinishTrigger() => _player.AnimationFinishTrigger();

        private void FootstepTrigger() => _player.FXController.FootstepSFX();
    }
}
