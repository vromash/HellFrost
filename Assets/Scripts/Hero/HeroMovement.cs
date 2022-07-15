using UnityEngine;

namespace Hero
{
    public class HeroMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float speedLimiter;

        private Rigidbody2D _rb;
        private float _inputHorizontal;
        private float _inputVertical;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
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
            }
            else
            {
                _rb.velocity = new Vector2(0, 0);
            }
        }
    }
}
