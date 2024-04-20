using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Timer
    {
        public event Action OnTimerDone;

        private float startTime;
        private readonly float duration;
        private float targetTime;

        private bool isActive;

        public Timer()
        {
        }

        public Timer(float duration)
        {
            this.duration = duration;
        }

        public void StartTimer()
        {
            isActive = true;
            startTime = Time.time;
            targetTime = startTime + duration;
        }
        public void StartTimer(float duration)
        {
            isActive = true;
            startTime = Time.time;
            targetTime = startTime + duration;
        }

        public void StopTimer()
        {
            isActive = false;
        }

        public void Tick()
        {
            if (!isActive) return;

            if (Time.time > targetTime) 
            {
                OnTimerDone?.Invoke();
                StopTimer();
            }
        }
    }
}
