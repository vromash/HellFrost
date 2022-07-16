using UnityEngine;

namespace Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private int speed;
        [SerializeField] private bool holdsDistance;
        [SerializeField] private float stoppingDistance;
        [SerializeField] private float retreatDistance;

        private Rigidbody2D _rb;
        private Transform _target;
        private Vector2 _direction;
        private EnemyVisuals _visuals;
        private bool _toMove;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _visuals = GetComponent<EnemyVisuals>();
        }

        public void Initialize(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            _direction = _target.position - transform.position;
            _toMove = true;
            if (!holdsDistance) return;

            var distanceToTarget = Vector2.Distance(transform.position, _target.position);

            if (distanceToTarget < retreatDistance)
            {
                _direction *= -1;
            }
            else if (distanceToTarget < stoppingDistance && distanceToTarget > retreatDistance)
            {
                _toMove = false;
                _direction = Vector2.zero;
            }
        }

        private void FixedUpdate()
        {
            if (holdsDistance)
            {
                if (_toMove)
                    _visuals.SetRunAnimation();
                else
                    _visuals.SetIdleAnimation();
            }

            _rb.velocity = new Vector2(_direction.x, _direction.y).normalized * speed;
        }
    }
}
