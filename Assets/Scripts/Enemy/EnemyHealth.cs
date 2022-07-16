using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        public event Action<GameObject> onDied;

        [SerializeField] private int maxHealth;
        private int _health;

        public void Initialize()
        {
            _health = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            _health -= amount <= 0 ? 1 : amount;
            if (_health <= 0)
            {
                _health = 0;
                Die();
            }
        }

        private void Die()
        {
            onDied?.Invoke(gameObject);
        }
    }
}
