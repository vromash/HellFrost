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

        public GameObject Get()
        {
            return _pool.Get();
        }

        public void Release(GameObject enemy)
        {
            _pool.Release(enemy);
        }

        private GameObject Create()
        {
            var enemy = Instantiate(prefab, spawnParent.position, Quaternion.identity, spawnParent);
            enemy.GetComponent<EnemyMovement>().Initialize(target);

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
