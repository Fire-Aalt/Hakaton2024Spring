using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class TouchButton : Button, IPointerDownHandler, IPointerUpHandler
    {
        public UnityEvent OnButtonDown, OnButtonUp;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            OnButtonDown?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            OnButtonUp?.Invoke();
        }
    }
}
