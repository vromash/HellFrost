using UI;
using UnityEngine;
using UnityEngine.Pool;

namespace Dice
{
    public class DicePool : MonoBehaviour
    {
        [SerializeField] private DiceTray diceTray;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform spawnParent;
        [SerializeField] private DamageNumberPool damageNumberPool;

        private ObjectPool<GameObject> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<GameObject>(Create, OnGet, OnRelease);
        }

        public GameObject Get(Vector2 position)
        {
            var dice = diceTray.Get();
            var diceGO = _pool.Get();

            diceGO.layer = LayerMask.NameToLayer("Projectile");
            diceGO.transform.position = position;
            diceGO.GetComponent<DiceProjectile>().Initialize(dice.Value(), dice.ShapeValue());

            return diceGO;
        }

        public GameObject Get(Vector2 position, int damage, int maxDamage)
        {
            var diceGO = _pool.Get();

            diceGO.layer = LayerMask.NameToLayer("EnemyProjectile");
            diceGO.transform.position = position;
            diceGO.GetComponent<DiceProjectile>().Initialize(damage, maxDamage);

            return diceGO;
        }

        private void Release(GameObject dice)
        {
            _pool.Release(dice);
        }

        private GameObject Create()
        {
            var dice = Instantiate(prefab, spawnParent.position, Quaternion.identity, spawnParent);
            dice.GetComponent<DiceProjectile>().onDestroy += Release;
            dice.GetComponent<DiceProjectile>().onHit += damageNumberPool.Spawn;
            return dice;
        }

        private void OnGet(GameObject dice)
        {
            dice.SetActive(true);
        }

        private void OnRelease(GameObject dice)
        {
            // dice.SetActive(false);
        }
    }
}
