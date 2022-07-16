using System;
using System.Collections.Generic;
using Dice;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    [Serializable]
    public struct EnemyWave
    {
        public EnemyTypeChance[] types;
        public float waveDuration;
        public int waveRepeatTimes;
        public int enemyNumber;
    }

    [Serializable]
    public struct EnemyTypeChance
    {
        public EnemyType enemyType;
        public float chance;
    }

    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyWave[] waves;
        [SerializeField] private BoxCollider2D[] spawnAreas;
        [SerializeField] private EnemyPool enemyPool;

        [SerializeField] private float minSpawnCooldown;
        [SerializeField] private float minConcurrentSpawnNumber;
        [SerializeField] private float maxConcurrentSpawnNumber;

        private readonly Queue<EnemyType> _enemiesToSpawn = new();
        private readonly List<GameObject> _aliveEnemies = new();

        private int _currentWaveNumber;
        private EnemyWave _currentWave;

        private float _spawnCooldownTimer;
        private float _previousSpawnCooldown;
        private float _initialSpawnCooldown;
        private float _spawnCooldownModifier;

        private void Start()
        {
            PrepareWave();
        }

        private void Update()
        {
            if (_spawnCooldownTimer > 0)
            {
                _spawnCooldownTimer -= Time.deltaTime;
                return;
            }

            if (_enemiesToSpawn.Count != 0)
            {
                SpawnEnemies();
            }
        }

        private void PrepareWave()
        {
            _currentWaveNumber++;

            var waveToChoose = _currentWaveNumber;
            if (waveToChoose > waves.Length)
                waveToChoose = waves.Length;

            _currentWave = waves[waveToChoose - 1];
            for (var i = 0; i < _currentWave.enemyNumber; i++)
            {
                foreach (var typeChance in _currentWave.types)
                {
                    if (Random.Range(0, 101) > typeChance.chance)
                        continue;

                    _enemiesToSpawn.Enqueue(typeChance.enemyType);
                    break;
                }
            }

            PrepareCooldown();
        }

        private void PrepareCooldown()
        {
            _initialSpawnCooldown = _currentWave.waveDuration / _currentWave.enemyNumber;
            _spawnCooldownModifier = _initialSpawnCooldown / _currentWave.enemyNumber;

            _spawnCooldownTimer = _initialSpawnCooldown;
            _previousSpawnCooldown = _initialSpawnCooldown;
        }

        private void SpawnEnemies()
        {
            var enemyNumber = Random.Range(minConcurrentSpawnNumber, maxConcurrentSpawnNumber);
            for (var i = 0; i < enemyNumber; i++)
                SpawnEnemy();

            RecalculateSpawnCooldown();
        }

        private void SpawnEnemy()
        {
            if (_enemiesToSpawn.Count == 0)
                return;

            var spawnPosition = GetSpawnPosition();
            var enemyType = _enemiesToSpawn.Dequeue();
            var enemyGO = enemyPool.Get(enemyType);

            enemyGO.transform.position = spawnPosition;
            enemyGO.GetComponent<EnemyHealth>().onDied += RegisterDiedEnemy;

            _aliveEnemies.Add(enemyGO);
        }

        private void RecalculateSpawnCooldown()
        {
            _spawnCooldownTimer = _previousSpawnCooldown - _spawnCooldownModifier;
            if (_spawnCooldownTimer <= 0 || _spawnCooldownTimer <= minSpawnCooldown)
                _spawnCooldownTimer = minSpawnCooldown;

            _previousSpawnCooldown = _spawnCooldownTimer;
        }

        private void RegisterDiedEnemy(GameObject enemyGO)
        {
            enemyGO.GetComponent<EnemyHealth>().onDied -= RegisterDiedEnemy;
            _aliveEnemies.Remove(enemyGO);
        }

        private Vector3 GetSpawnPosition()
        {
            return GetRandomPointInsideCollider(spawnAreas[Random.Range(0, spawnAreas.Length)]);
        }

        private Vector3 GetRandomPointInsideCollider(BoxCollider2D boxCollider)
        {
            Vector3 extents = boxCollider.size / 2f;
            Vector3 point = new Vector3(
                Random.Range(-extents.x, extents.x),
                Random.Range(-extents.y, extents.y),
                Random.Range(-extents.z, extents.z));
            // ) + boxCollider.offset;
            return boxCollider.transform.TransformPoint(point);
        }
    }
}
