using System;
using System.Collections;
using UI;
using UnityEngine;

namespace Hero
{
    public class HeroHealth : MonoBehaviour
    {
        public event Action onHealthUpdated;

        [SerializeField] private InGameUI inGameUI;
        [SerializeField] private int maxHealth;
        [SerializeField] private float invincibilityTime;

        private HeroVisuals _visuals;
        private PolygonCollider2D _collider2D;
        private int _health;

        private void Awake()
        {
            _health = maxHealth;
            _visuals = GetComponent<HeroVisuals>();
            _collider2D = GetComponent<PolygonCollider2D>();
        }

        public void TakeDamage(int amount)
        {
            _health -= amount <= 0 ? 1 : amount;
            if (_health <= 0)
            {
                _health = 0;
                Die();
            }

            onHealthUpdated?.Invoke();
        }

        public void Heal(int amount)
        {
            _health += amount;
            if (_health > 50)
                _health = 50;

            onHealthUpdated?.Invoke();
        }

        public IEnumerator BecomeInvincible()
        {
            _collider2D.enabled = false;
            _visuals.ToggleInvisible();
            yield return new WaitForSeconds(invincibilityTime);
            _visuals.ToggleInvisible();
            _collider2D.enabled = true;
        }

        private void Die()
        {
            inGameUI.ToggleDeathMenu();
        }

        public int Health() => _health;
        public int MaxHealth() => maxHealth;
    }
}
