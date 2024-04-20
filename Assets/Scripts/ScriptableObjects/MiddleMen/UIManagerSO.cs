using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Managers/UIManager", fileName = "UIManager")]
    public class UIManagerSO : ScriptableObject
    {
        public event Action<float, Vector2> OnStartInteractionProgressBar;
        public event Action<bool> OnStopInteractionProgressBar;

        public void StartInteractionProgressBar(float duration, Vector2 startPos) => OnStartInteractionProgressBar?.Invoke(duration, startPos);
        public void StopInteractionProgressBar(bool forceStop) => OnStopInteractionProgressBar?.Invoke(forceStop);
    }
}