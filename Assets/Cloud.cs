using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Cloud : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Vector2 _sizeInterval;
        [SerializeField] private Vector2 _speedInterval;
        [SerializeField] private Vector2 _opacityInterval;

        private float _speed;
        private float _minX;

        public void Init(float minX)
        {
            _minX = minX;
            float opacity = Random.Range(_sizeInterval.x, _sizeInterval.y);
            float speed = Random.Range(_speedInterval.x, _speedInterval.y);
            float size = Random.Range(_opacityInterval.x, _opacityInterval.y);

            transform.localScale = new Vector2(transform.localScale.x * size, transform.localScale.y * size);
            _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, opacity);
            _speed = speed;
        }

        void Update()
        {
            transform.localPosition = new Vector3(transform.localPosition.x - _speed * Time.deltaTime, transform.localPosition.y);
            if (transform.localPosition.x < _minX)
            {
                Destroy(gameObject);
            }
        }
    }
}
