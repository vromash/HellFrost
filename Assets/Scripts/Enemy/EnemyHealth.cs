using System;
using System.Collections;
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
        [SerializeField] private AudioSource _audioSourceHit;
        [SerializeField] private AudioSource _audioSourceEat;

        private EnemyMovement _enemyMovement;
        private EnemyAttack _enemyAttack;
        private EnemyVisuals _enemyVisuals;
        private EnemyInventory _enemyInventory;
        private int _health;

        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
            _enemyAttack = GetComponent<EnemyAttack>();
            _enemyVisuals = GetComponent<EnemyVisuals>();
            _enemyInventory = GetComponent<EnemyInventory>();
        }

        public void Initialize()
        {
            _health = maxHealth;
        }

        public bool ResistsEven(int damage) => resistsEven && damage <= 12;
        public bool ResistsOdd() => resistsOdd;

        public IEnumerator Freeze(float time)
        {
            _enemyMovement.enabled = false;
            _enemyAttack.enabled = false;
            yield return new WaitForSeconds(time);
            _enemyMovement.enabled = true;
            _enemyAttack.enabled = true;
        }

        public void TakeDamage(int amount)
        {
            if (amount != 20 && amount % 2 == 0 && absorbEven)
            {
                _audioSourceEat.Play();
                Heal(amount);
                return;
            }

            _audioSourceHit.Play();

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
