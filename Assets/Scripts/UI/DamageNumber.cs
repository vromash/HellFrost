using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public enum FontColor
    {
        White,
        Red
    }

    public class DamageNumber : MonoBehaviour
    {
        public event Action<GameObject> onFadeEnd;

        private TMP_Text _text;

        [SerializeField] private Color endColor;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float fadeDuration;

        [SerializeField] private TMP_FontAsset whiteFont;
        [SerializeField] private TMP_FontAsset redFont;

        private float _fadeStartTime;
        private Color _initialColor;
        private Vector3 _initialPosition;
        private Vector3 _finalPosition;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        public void Activate(Vector2 position, int damage, FontColor color)
        {
            var fontAsset = color switch
            {
                FontColor.White => whiteFont,
                FontColor.Red => redFont,
                _ => whiteFont
            };

            _text.font = fontAsset;
            _initialColor = fontAsset.material.GetColor("_FaceColor");
            _text.text = $"{damage}";

            _fadeStartTime = Time.time;
            _initialPosition = position;
            _finalPosition = _initialPosition + offset;
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
