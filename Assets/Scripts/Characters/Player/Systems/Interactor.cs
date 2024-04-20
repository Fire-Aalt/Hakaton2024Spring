using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Interactor : MonoBehaviour
    {
        [Title("Data")]
        [SerializeField] private UIManagerSO _UIManager;

        [Title("References")]
        [SerializeField] private Transform _center;

        [Title("Settings")]
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactableLayer;
        [SerializeField] private bool _drawGizmos;
        public bool IsInteracting => _isInteracting;

        private IInteractable _currentInteractable;
        private Vector2 _currentInteractablePosition;

        private IInteractable _closestInteractable;
        private bool _isInteracting;
        private bool _interactInput;

        private Collider2D[] _results;

        private void Start()
        {
            _results = new Collider2D[100];
        }

        private void Update()
        {
            if (_closestInteractable != null && _interactInput && !_isInteracting)
            {
                _currentInteractable = _closestInteractable;

                if (!_currentInteractable.CanInteract)
                    return;

                if (_currentInteractable.HoldInteract)
                {
                    StartCoroutine(HoldInteractionCoroutine(_currentInteractable.HoldDuration));
                }
                else
                {
                    Interact();
                }
            }
        }

        private void FixedUpdate()
        {
            SearchForInteractable();
        }

        private void SearchForInteractable()
        {
            int count = Physics2D.OverlapCircleNonAlloc(_center.position, _interactionRadius, _results, _interactableLayer);

            float minDistance = _interactionRadius;
            _closestInteractable = null;

            for (int i = 0; i < count; i++)
            {
                Collider2D collider = _results[i];
                if (collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    float distance = Vector2.Distance(_center.position, collider.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        _closestInteractable = interactable;
                        _currentInteractablePosition = collider.transform.position;
                    }
                }
            }
        }

        private IEnumerator HoldInteractionCoroutine(float holdDuration)
        {
            _isInteracting = true;
            _currentInteractable.StartHold();

            PlayerInputHandler.Current.SetInputState(false);

            _UIManager.StartInteractionProgressBar(_currentInteractable.HoldDuration, _currentInteractablePosition);

            float timer = 0f;
            while (timer < holdDuration && _interactInput && _currentInteractable == _closestInteractable)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            if (_interactInput && _currentInteractable == _closestInteractable)
            {
                Interact();
            }
            else
            {
                _currentInteractable.FailHold();
                _UIManager.StopInteractionProgressBar(true);
            }

            PlayerInputHandler.Current.SetInputState(true);
            _isInteracting = false;
        }

        private void Interact()
        {
            _currentInteractable.Interact();
            _currentInteractable = null;
        }

        private void OnDrawGizmos()
        {
            if (_drawGizmos)
            {
                Gizmos.DrawWireSphere(_center.position, _interactionRadius);
            }
        }

        private void OnInteractStart() => _interactInput = true;

        private void OnInteractCancel() => _interactInput = false;

        private void OnEnable()
        {
            InputManager.InteractEvent += OnInteractStart;
            InputManager.InteractCancelEvent += OnInteractCancel;
        }

        private void OnDisable()
        {
            InputManager.InteractEvent -= OnInteractStart;
            InputManager.InteractCancelEvent -= OnInteractCancel;
        }
    }
}