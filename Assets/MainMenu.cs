using MoreMountains.Tools;
using RenderDream.GameEssentials;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private MMTouchButton _startButton;

        private void Start()
        {
            if (!InputManager.TouchInputEnabled)
            {
                _startButton.MouseMode = true;
            }
        }

        public void StartGame()
        {
            _startButton.Interactable = false;
            SceneLoader.Current.LoadSceneWithTransition(SceneType.Gameplay);
        }
    }
}
