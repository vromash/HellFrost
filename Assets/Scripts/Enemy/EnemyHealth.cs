using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        public event Action<GameObject> onDied;

        [SerializeField] private int maxHealth;
        [SerializeField] private bool resistsEven;
        [SerializeField] private bool resistsOdd;
        [SerializeField] private bool absorbEven;

        private EnemyInventory _enemyInventory;
        private int _health;

        private void Awake()
        {
            _enemyInventory = GetComponent<EnemyInventory>();
        }

        public void Initialize()
        {
            _health = maxHealth;
        }

        public bool ResistsEven() => resistsEven;
        public bool ResistsOdd() => resistsOdd;

        public void TakeDamage(int amount)
        {
            if (amount % 2 == 0 && absorbEven)
            {
                Heal(amount);
                return;
            }

            _health -= amount <= 0 ? 1 : amount;
            if (_health <= 0)
            {
                _health = 0;
                Die();
            }
        }

        private void Heal(int amount)
        {
            _health += amount;
            transform.localScale = new Vector3(transform.localScale.x + 0.2f, transform.localScale.y + 0.2f,
                transform.localScale.z + 0.2f);
        }

        private void Die()
        {
            _enemyInventory.GiveCoins();
            onDied?.Invoke(gameObject);
        }
    }
}
