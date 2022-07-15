using UnityEngine;

namespace Hero
{
    public class Ability : MonoBehaviour
    {
        [SerializeField] private float cooldown;

        private bool _allowedToUseAbility;
        private float _cooldownTimer;

        private void Update()
        {
            if (!_allowedToUseAbility)
            {
                TickCooldown();
                return;
            }

            if (Input.GetMouseButton(1))
            {
                _allowedToUseAbility = false;
                Debug.Log("ability used");
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
