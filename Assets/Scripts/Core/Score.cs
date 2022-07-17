using TMPro;
using UnityEngine;

namespace Core
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private TMP_Text display;
        [SerializeField] private TMP_Text deathMenuCurrentScore;
        [SerializeField] private TMP_Text deathMenuBestScore;

        private int _bestScore;
        private int _currentScore;

        private void Start()
        {
            _bestScore = PlayerPrefs.GetInt("bestScore");
            deathMenuBestScore.text = _bestScore.ToString();
            UpdateDisplay();
        }

        public void SaveScore()
        {
            PlayerPrefs.SetInt("bestScore", _currentScore);
            PlayerPrefs.Save();
        }

        public void IncreaseScore(int amount)
        {
            _currentScore += amount;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            display.text = $"score: {_currentScore}";
            deathMenuCurrentScore.text = _currentScore.ToString();

            if (_currentScore > _bestScore)
                deathMenuBestScore.text = _currentScore.ToString();
        }
    }
}
