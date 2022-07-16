using UnityEngine;

namespace Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private int speed;

        private Rigidbody2D _rb;
        private Transform _target;
        private Vector2 _direction;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            _direction = _target.position - transform.position;
        }

        private void FixedUpdate()
        {
            _rb.velocity = new Vector2(_direction.x, _direction.y).normalized * speed;
        }
    }
}
