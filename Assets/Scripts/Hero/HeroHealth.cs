using System;
using UnityEngine;

namespace Hero
{
    public class HeroHealth : MonoBehaviour
    {
        public event Action onHealthUpdated;

        [SerializeField] private int maxHealth;
        private int _health;

        private void Awake()
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

            onHealthUpdated?.Invoke();;
        }

        private void Die()
        {
        }

        public int Health() => _health;
        public int MaxHealth() => maxHealth;
    }
}
