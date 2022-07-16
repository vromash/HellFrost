using Dice;
using UnityEngine;

namespace Hero
{
    public class HeroVisuals : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
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
    }
}
