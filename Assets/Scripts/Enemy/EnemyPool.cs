using Enemy;
using UnityEngine;
using UnityEngine.Pool;

namespace Dice
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform spawnParent;
        [SerializeField] private Transform target;

        private ObjectPool<GameObject> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<GameObject>(Create, OnGet, OnRelease);
        }

        private void Update()
        {
            if (Input.GetKeyDown("e"))
            {
                Get();
            }
        }

        public GameObject Get(EnemyType enemyType)
        {
            var enemy = _pool.Get();
            enemy.GetComponent<EnemyHealth>().Initialize();
            return enemy;
        }


        public GameObject Get()
        {
            var enemy = _pool.Get();
            enemy.GetComponent<EnemyHealth>().Initialize();
            return enemy;
        }

        public void Release(GameObject enemy)
        {
            _pool.Release(enemy);
        }

        private GameObject Create()
        {
            var enemy = Instantiate(prefab, spawnParent.position, Quaternion.identity, spawnParent);
            enemy.GetComponent<EnemyMovement>().Initialize(target);
            enemy.GetComponent<EnemyHealth>().onDied += Release;
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
