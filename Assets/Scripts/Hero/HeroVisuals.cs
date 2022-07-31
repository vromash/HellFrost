using Dice;
using UnityEngine;

namespace Hero
{
    public class HeroVisuals : MonoBehaviour
    {
        [SerializeField] private Color invisibilityColor;

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        private bool _isInvisible;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetRunAnimation(DiceElement el)
        {
            string state = el switch
            {
                DiceElement.Fire => "RunFire",
                DiceElement.Frost => "RunFrost",
                _ => ""
            };

            _animator.SetTrigger(state);
        }

        public void SetIdleAnimation(DiceElement el)
        {
            string state = el switch
            {
                DiceElement.Fire => "IdleFire",
                DiceElement.Frost => "IdleFrost",
                _ => ""
            };

            _animator.SetTrigger(state);
        }

        public void SetThrowAnimation()
        {
            _animator.SetTrigger("Throw");
        }

        public void ToggleInvisible()
        {
            _spriteRenderer.color = _isInvisible ? Color.white : invisibilityColor;
            _isInvisible = !_isInvisible;
        }
    }
}
