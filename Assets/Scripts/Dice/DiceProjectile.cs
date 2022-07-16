using System;
using Enemy;
using UnityEngine;

namespace Dice
{
    public class DiceProjectile : MonoBehaviour
    {
        public event Action<Vector2, int> onHit;
        public event Action<GameObject> onDestroy;

        [SerializeField] private float force;
        [SerializeField] private float ttl;

        private Rigidbody2D _rb;
        private int _damage;
        private float _lifeTimer;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _lifeTimer += Time.deltaTime;
            if (_lifeTimer >= ttl)
                Destroy();
        }

        public void SetDamage(int damage) => _damage = damage;

        public void Instantiate(Vector3 mousePosition)
        {
            var position = transform.position;

            var direction = mousePosition - position;
            _rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

            var rotation = position - mousePosition;
            var rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotationZ + 90);

            _lifeTimer = 0;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                col.gameObject.GetComponent<EnemyHealth>().TakeDamage(_damage);
                Hit();
            }
        }

        private void Hit()
        {
            onHit?.Invoke(transform.position, _damage);
            Destroy();
        }

        private void Destroy()
        {
            onDestroy?.Invoke(gameObject);
        }
    }
}
