using UnityEngine;

namespace Dice
{
    public class DiceProjectile : MonoBehaviour
    {
        [SerializeField] private float force;

        private Rigidbody2D _rb;
        private int _damage;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
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
        }
    }
}
