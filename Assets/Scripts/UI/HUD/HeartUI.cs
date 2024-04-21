using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HeartUI : MonoBehaviour
    {
        [Header("MMF Players")]
        [SerializeField] private MMF_Player _healPlayer;
        [SerializeField] private MMF_Player _hurtPlayer;

        [SerializeField] private Sprite _filledSprite;
        [SerializeField] private Sprite _emptySprite;

        public FillState FillState { get; private set; }
        public float TransitionDuration { get; private set; }
        public RectTransform RectTransform { get; private set; }

        private Image _image;

        public void Initialize(FillState fillState)
        {
            _image = GetComponent<Image>();
            RectTransform = GetComponent<RectTransform>();

            _healPlayer.Initialization();
            _hurtPlayer.Initialization();

            switch (fillState)
            {
                case FillState.Empty:
                    _image.sprite = _emptySprite;
                    TransitionDuration = _hurtPlayer.TotalDuration;
                    break;
                case FillState.Full:
                    _image.sprite = _filledSprite;
                    TransitionDuration = _healPlayer.TotalDuration;
                    break;
            }
            FillState = fillState;
        }

        public void Heal()
        {
            _hurtPlayer.StopFeedbacks();
            TransitionDuration = _healPlayer.TotalDuration;
            _healPlayer.PlayFeedbacks();
            _image.sprite = _filledSprite;
            FillState = FillState.Full;
        }

        public void Hurt()
        {
            _healPlayer.StopFeedbacks();
            TransitionDuration = _hurtPlayer.TotalDuration;
            _hurtPlayer.PlayFeedbacks();
            _image.sprite = _emptySprite;
            FillState = FillState.Empty;
        }
    }

    public enum FillState
    {
        Empty,
        Full
    }
}
