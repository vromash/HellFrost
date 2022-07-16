using Dice;
using UnityEngine;

namespace Hero
{
    public class HeroMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float speedLimiter;

        private HeroVisuals _visuals;
        private Rigidbody2D _rb;
        private float _inputHorizontal;
        private float _inputVertical;
        private DiceElement _element;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _visuals = GetComponent<HeroVisuals>();
        }

        private void Update()
        {
            _inputHorizontal = Input.GetAxisRaw("Horizontal");
            _inputVertical = Input.GetAxisRaw("Vertical");
        }

        private void FixedUpdate()
        {
            if (_inputHorizontal != 0 || _inputVertical != 0)
            {
                if (_inputHorizontal != 0 && _inputVertical != 0)
                {
                    _inputHorizontal *= speedLimiter;
                    _inputVertical *= speedLimiter;
                }

                _rb.velocity = new Vector2(_inputHorizontal, _inputVertical) * speed;
                _visuals.SetRunAnimation(_element);
            }
            else
            {
                _visuals.SetIdleAnimation(_element);
                _rb.velocity = new Vector2(0, 0);
            }
        }

        public void UpdateDiceElement(DiceElement element) => _element = element;
    }
}
