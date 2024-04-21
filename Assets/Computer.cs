using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Computer : SerializedMonoBehaviour
    {
        [SerializeField] Image empty, full;
        [SerializeField] Dictionary<ComputerPart, Image> dict = new();
        public static int parts = 0;

        public void OnPartAdded(ComputerPart part)
        {
            dict[part].gameObject.SetActive(true);
            parts++;
            if (parts == 4)
            {
                foreach (var item in dict.Values)
                {
                    item.gameObject.SetActive(false);
                }
                empty.gameObject.SetActive(false);
                full.gameObject.SetActive(true);
            }
        }

        private void OnEnable()
        {
            InteractableStation.OnInteract += OnPartAdded;
        }

        private void OnDisable()
        {
            InteractableStation.OnInteract -= OnPartAdded;
        }
    }
}
