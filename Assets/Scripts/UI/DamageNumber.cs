using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DamageNumber : MonoBehaviour
    {
        public event Action<GameObject> onFadeEnd;

        private TMP_Text _text;

        [SerializeField] private Color endColor;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float fadeDuration;

        private float _fadeStartTime;
        private Color _initialColor;
        private Vector3 _initialPosition;
        private Vector3 _finalPosition;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _initialColor = _text.color;
        }

        public void Activate(Vector2 position, int damage)
        {
            _fadeStartTime = Time.time;
            _initialPosition = position;
            _finalPosition = _initialPosition + offset;
            _text.text = $"{damage}";
        }

        private void Update()
        {
            var progress = (Time.time - _fadeStartTime) / fadeDuration;
            if (progress <= 1)
            {
                transform.localPosition = Vector3.Lerp(_initialPosition, _finalPosition, progress);
                _text.color = Color.Lerp(_initialColor, endColor, progress);
            }
            else
            {
                onFadeEnd?.Invoke(gameObject);
            }
        }
    }
}
