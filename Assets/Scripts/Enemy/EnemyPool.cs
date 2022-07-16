using System;
using Enemy;
using UI;
using UnityEngine;
using UnityEngine.Pool;

namespace Dice
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private GameObject prefabGoblin;
        [SerializeField] private GameObject prefabBeholder;
        [SerializeField] private GameObject prefabSkeleton;
        [SerializeField] private GameObject prefabOgre;
        [SerializeField] private GameObject prefabCube;
        [SerializeField] private Transform spawnParent;
        [SerializeField] private Transform target;
        [SerializeField] private DicePool dicePool;
        [SerializeField] private DamageNumberPool damageNumberPool;

        private ObjectPool<GameObject> _poolGoblin;
        private ObjectPool<GameObject> _poolBeholder;
        private ObjectPool<GameObject> _poolSkeleton;
        private ObjectPool<GameObject> _poolOgre;
        private ObjectPool<GameObject> _poolCube;

        private void Awake()
        {
            _poolGoblin = new ObjectPool<GameObject>(CreateGoblin, OnGet, OnRelease);
            _poolBeholder = new ObjectPool<GameObject>(CreateBeholder, OnGet, OnRelease);
            _poolSkeleton = new ObjectPool<GameObject>(CreateSkeleton, OnGet, OnRelease);
            _poolOgre = new ObjectPool<GameObject>(CreateOgre, OnGet, OnRelease);
            _poolCube = new ObjectPool<GameObject>(CreateCube, OnGet, OnRelease);
        }

        public GameObject Get(EnemyType enemyType)
        {
            GameObject enemy = enemyType switch
            {
                EnemyType.Goblin => _poolGoblin.Get(),
                EnemyType.Beholder => _poolBeholder.Get(),
                EnemyType.Skeleton => _poolSkeleton.Get(),
                EnemyType.Ogre => _poolOgre.Get(),
                EnemyType.Cube => _poolCube.Get(),
                _ => _poolGoblin.Get()
            };

            enemy.GetComponent<EnemyHealth>().Initialize();
            enemy.GetComponent<EnemyAttack>().onHit += damageNumberPool.Spawn;
            enemy.GetComponent<EnemyAttack>().onThrew += dicePool.Get;
            return enemy;
        }

        private void ReleaseGoblin(GameObject enemy) => _poolGoblin.Release(enemy);
        private void ReleaseBeholder(GameObject enemy) => _poolBeholder.Release(enemy);
        private void ReleaseSkeleton(GameObject enemy) => _poolSkeleton.Release(enemy);
        private void ReleaseOgre(GameObject enemy) => _poolOgre.Release(enemy);
        private void ReleaseCube(GameObject enemy) => _poolCube.Release(enemy);

        private GameObject CreateGoblin() => Create(prefabGoblin, ReleaseGoblin);
        private GameObject CreateBeholder() => Create(prefabBeholder, ReleaseBeholder);
        private GameObject CreateSkeleton() => Create(prefabSkeleton, ReleaseSkeleton);
        private GameObject CreateOgre() => Create(prefabOgre, ReleaseOgre);
        private GameObject CreateCube() => Create(prefabCube, ReleaseCube);

        private GameObject Create(GameObject enemyPrefab, Action<GameObject> releaseFunc)
        {
            var enemy = Instantiate(enemyPrefab, spawnParent.position, Quaternion.identity, spawnParent);
            enemy.GetComponent<EnemyMovement>().Initialize(target);
            enemy.GetComponent<EnemyAttack>().Initialize(target);
            enemy.GetComponent<EnemyHealth>().onDied += releaseFunc;
            return enemy;
        }

        private void OnGet(GameObject enemy)
        {
            enemy.SetActive(true);
        }

        private void OnRelease(GameObject enemy)
        {
            enemy.SetActive(false);
        }
    }
}
