using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Transformator : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] private Hitbox hitbox;
        [SerializeField] private float interval;
        private Timer _timer;


        private void Start()
        {
            _timer = new Timer(interval);
            _timer.OnTimerDone += Trigger;
            hitbox.IsEnabled = false;
            _timer.StartTimer();
        }
        private void Update()
        {
            _timer.Tick();
        }

        private void Trigger()
        {
            hitbox.IsEnabled = true;
            anim.SetTrigger("Trigger");
            _timer.StartTimer();
        }

        public void StopParticle()
        {
            hitbox.IsEnabled = false;
        }
    }
}
