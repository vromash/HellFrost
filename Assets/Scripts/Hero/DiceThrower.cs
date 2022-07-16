using Dice;
using UnityEngine;

namespace Hero
{
    public class DiceThrower : MonoBehaviour
    {
        [SerializeField] private Transform diceSpawnPosition;
        [SerializeField] private DicePool dicePool;
        [SerializeField] private float cooldown;

        private Camera _mainCamera;
        private Vector3 _mousePosition;

        private bool _allowedToThrow;
        private float _cooldownTimer;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            _mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

            var rotation = _mousePosition - transform.position;
            var rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, rotationZ);

            if (!_allowedToThrow)
            {
                TickCooldown();
                return;
            }

            if (Input.GetMouseButtonDown(0) && _allowedToThrow)
            {
                _allowedToThrow = false;
                var dice = dicePool.Get(diceSpawnPosition.position);
                dice.GetComponent<DiceProjectile>().Throw(_mousePosition);
            }
        }

        private void TickCooldown()
        {
            _cooldownTimer += Time.deltaTime;

            if (_cooldownTimer < cooldown) return;

            _allowedToThrow = true;
            _cooldownTimer = 0;
        }
    }
}
