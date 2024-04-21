using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class InteractableStation : MonoBehaviour, IInteractable
    {
        public static event Action<ComputerPart> OnInteract;

        [SerializeField] private Canvas canvas;
        [SerializeField] private Slider slider;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer textRenderer;
        [SerializeField] private SpriteRenderer stationRenderer;
        [SerializeField] private SpriteRenderer signRenderer;
        [SerializeField] private MMTouchButton button;
        [SerializeField] private ComputerPart computerPart;

        [SerializeField] private Sprite repairSprite;
        [SerializeField] private Sprite clickSprite;
        [SerializeField] private float holdDuration;

        [SerializeField] private Sprite completeSprite;

        public bool HoldInteract => true;

        public float HoldDuration => holdDuration;

        public bool CanInteract => canInteract;
        private bool canInteract = true;

        public string CannotInteractText => throw new System.NotImplementedException();

        private float timeLeft;

        private void Start()
        {
            slider.gameObject.SetActive(false);

            if (InputManager.TouchInputEnabled)
            {
                button.MouseMode = false;
            }
            else
            {
                button.MouseMode = true;
            }
            canvas.worldCamera = Camera.main;
        }

        private void Update()
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;

                slider.value = 1 - timeLeft / holdDuration;
            }
        }

        public void FailHold()
        {
            textRenderer.sprite = clickSprite;
            slider.gameObject.SetActive(false);
            animator.SetBool("Repair", false);
            timeLeft = 0;
        }

        public void Interact()
        {
            stationRenderer.sprite = completeSprite;
            slider.gameObject.SetActive(false);
            textRenderer.gameObject.SetActive(false);
            canInteract = false;
            animator.SetBool("Repair", false);
            signRenderer.gameObject.SetActive(false);
            OnInteract?.Invoke(computerPart);
        }

        public void StartHold()
        {
            textRenderer.sprite = repairSprite;
            timeLeft = holdDuration;
            slider.gameObject.SetActive(true);
            animator.SetBool("Repair", true);
        }

        public void SimulateTouchDown()
        {
            InputManager.Current.InteractInput(UnityEngine.InputSystem.InputActionPhase.Performed);
        }

        public void SimulateTouchUp()
        {
            InputManager.Current.InteractInput(UnityEngine.InputSystem.InputActionPhase.Canceled);
        }
    }

    public enum ComputerPart
    {
        Mouse,
        Block,
        Keyboard,
        Monitor
    }
}
