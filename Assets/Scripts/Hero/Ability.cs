using Dice;
using UnityEngine;

namespace Hero
{
    public class Ability : MonoBehaviour
    {
        [SerializeField] private DiceTray diceTray;
        [SerializeField] private DiceThrower diceThrower;
        [SerializeField] private HeroShield shield;
        [SerializeField] private HeroHealth health;
        [SerializeField] private HeroMovement movement;
        [SerializeField] private float cooldown;
        [SerializeField] private AudioSource _audioSourceEffect;

        private bool _allowedToUseAbility;
        private float _cooldownTimer;

        private void Update()
        {
            if (!_allowedToUseAbility)
            {
                TickCooldown();
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (diceTray.Next().HasAbility())
                    Activate();
            }
        }

        private void Activate()
        {
            _allowedToUseAbility = false;
            var dice = diceTray.Get();

            switch (dice.Ability())
            {
                case DiceAbility.Shield:
                    _audioSourceEffect.Play();
                    shield.Activate(ShieldType.All);
                    break;
                case DiceAbility.Wave:
                    break;
                case DiceAbility.Ricochet:
                case DiceAbility.Freeze:
                    _audioSourceEffect.Play();
                    diceThrower.Throw(true);
                    break;
                case DiceAbility.Heal:
                    _audioSourceEffect.Play();
                    health.Heal(dice.Value());
                    break;
                case DiceAbility.Dash:
                    _audioSourceEffect.Play();
                    StartCoroutine(movement.Dash());
                    break;
                case DiceAbility.Ghost:
                    _audioSourceEffect.Play();
                    StartCoroutine(health.BecomeInvincible());
                    break;
                case DiceAbility.Shotgun:
                    break;
                case DiceAbility.Lightning:
                    break;
                default:
                    health.Heal(dice.Value());
                    break;
            }
        }

        private void TickCooldown()
        {
            _cooldownTimer += Time.deltaTime;

            if (_cooldownTimer < cooldown) return;

            _allowedToUseAbility = true;
            _cooldownTimer = 0;
        }
    }
}
