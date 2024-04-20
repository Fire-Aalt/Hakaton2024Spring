using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CloudsBG : MonoBehaviour
    {
        [SerializeField] private Cloud _cloudPrefab;
        [SerializeField] private Vector2 _spawnInterval;

        [SerializeField] private Vector2 _xInterval;
        [SerializeField] private Vector2 _yInterval;

        private Timer _spawnTimer;

        void Start()
        {
            _spawnTimer = new Timer();
            _spawnTimer.OnTimerDone += SpawnCloud;

            SpawnCloud();
        }

        void Update()
        {
            _spawnTimer.Tick();
        }

        private void SpawnCloud()
        {
            Cloud cloud = Instantiate(_cloudPrefab);

            float y = Random.Range(_yInterval.x, _yInterval.y);

            cloud.transform.SetParent(transform);
            cloud.transform.localPosition = new Vector2(_xInterval.y, y);
            cloud.Init(_xInterval.x);

            float duration = Random.Range(_spawnInterval.x, _spawnInterval.y);
            _spawnTimer.StartTimer(duration);
        }
    }
}
