using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class CollectibleHandler : MonoBehaviour, ICollectible
    {
        public CollectibleType CollectibleType;
        public UnityEvent OnCollect;

        public CollectibleType Type => CollectibleType;

        public void Collect()
        {
            OnCollect?.Invoke();
        }
    }
}
