using UnityEngine;

namespace UI
{
    public class InGameCursor : MonoBehaviour
    {
        [SerializeField] private Texture2D[] textureArray;
        [SerializeField] private float frameRate;

        private float _frameTimer;
        private int _currentFrame;
        private int _frameCount;
        private Vector2 _cursorHotspot;


        private void Start()
        {
            _cursorHotspot = new Vector2(textureArray[0].height / 2, textureArray[0].height / 2);
            SetActiveCursorAnimation();
        }

        private void Update()
        {
            _frameTimer -= Time.deltaTime;
            if (_frameTimer <= 0f)
            {
                _frameTimer += frameRate;
                _currentFrame = (_currentFrame + 1) % _frameCount;
                SetCursor();
            }
        }

        private void SetActiveCursorAnimation()
        {
            _currentFrame = 0;
            _frameTimer = 0f;
            _frameCount = textureArray.Length;
        }

        private void SetCursor() => Cursor.SetCursor(textureArray[_currentFrame], _cursorHotspot, CursorMode.Auto);
    }
}
