using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ArcBalls : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Hitbox hitbox;
        [SerializeField] private float interval;
        [SerializeField] private float duration;
        private Timer _timer;
        private Timer _durationTimer;


        private void Start()
        {
            _timer = new Timer(interval);
            _durationTimer = new Timer(duration);
            _timer.OnTimerDone += Trigger;
            _durationTimer.OnTimerDone += StopParticle;
            hitbox.IsEnabled = false;
            sprite.enabled = false;
            _timer.StartTimer();
        }
        private void Update()
        {
            _timer.Tick();
            _durationTimer.Tick();
        }

        private void Trigger()
        {
            hitbox.IsEnabled = true;
            sprite.enabled = true;
            _durationTimer.StartTimer();
            _timer.StartTimer();
        }

        public void StopParticle()
        {
            hitbox.IsEnabled = false;
            sprite.enabled = false;
        }
    }
}
