using UnityEngine;
using UnityEngine.Pool;

namespace UI
{
    public class DamageNumberPool : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform spawnParent;

        private ObjectPool<GameObject> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<GameObject>(Create, OnGet, OnRelease);
        }

        public void Spawn(Vector2 position, int damage)
        {
            var number = _pool.Get();
            number.GetComponent<DamageNumber>().Activate(position, damage);
        }

        private void Release(GameObject number)
        {
            _pool.Release(number);
        }

        private GameObject Create()
        {
            var number = Instantiate(prefab, spawnParent.position, Quaternion.identity, spawnParent);
            number.GetComponent<DamageNumber>().onFadeEnd += Release;
            return number;
        }

        private void OnGet(GameObject number)
        {
            number.SetActive(true);
        }

        private void OnRelease(GameObject number)
        {
            number.SetActive(false);
        }
    }
}
