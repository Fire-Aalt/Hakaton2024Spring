using Game.CoreSystem;
using MoreMountains.Tools;
using RenderDream.GameEssentials;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DeathScreen : MonoBehaviour
    {
        [SerializeField] Canvas canvas;
        [SerializeField] MMTouchButton button;

        private void Start()
        {
            PlayerStats.OnPlayerDeath += ShowDeathScreen;
            if (InputManager.TouchInputEnabled)
            {
                button.MouseMode = false;
            }
            else
            {
                button.MouseMode = true;
            }
        }
        public void ShowDeathScreen()
        {
            int count = transform.childCount;
            for (int i = count - 1; i >= 0; i--) 
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }

            Player.Current.gameObject.SetActive(false);
            Player.Current.transform.position = new Vector2(4845, 4554);
            canvas.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public void Reload()
        {
            SceneLoader.Current.LoadSceneWithTransition(SceneType.MainMenu);
        }
    }
}
