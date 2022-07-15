using UnityEngine;

namespace Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private int speed;

        private Transform _target;

        public void Initialize(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
        }
    }
}
