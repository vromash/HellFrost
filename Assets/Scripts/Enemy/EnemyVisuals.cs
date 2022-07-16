using UnityEngine;

namespace Enemy
{
    public class EnemyVisuals : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetRunAnimation()
        {
            _animator.SetTrigger("Run");
        }

        public void SetIdleAnimation()
        {
            _animator.SetTrigger("Idle");
        }

        public void SetThrowAnimation()
        {
            _animator.SetTrigger("Throw");
        }
    }
}
