using Core;
using Hero;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthDisplay;
        [SerializeField] private GameObject pauseMenuPanel;
        [SerializeField] private GameObject deathMenuPanel;

        [SerializeField] private HeroHealth heroHealth;
        [SerializeField] private Score score;

        private void Start()
        {
            heroHealth.onHealthUpdated += UpdateHealthCounter;
            UpdateHealthCounter();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !deathMenuPanel.activeSelf)
            {
                TogglePauseMenu();
            }
        }

        private void TogglePauseMenu()
        {
            pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
            Time.timeScale = pauseMenuPanel.activeSelf ? 0f : 1f;
        }

        public void ToggleDeathMenu()
        {
            deathMenuPanel.SetActive(!deathMenuPanel.activeSelf);
            Time.timeScale = deathMenuPanel.activeSelf ? 0f : 1f;
            score.SaveScore();
        }

        private void UpdateHealthCounter()
        {
            healthDisplay.text = $"{heroHealth.Health()}/{heroHealth.MaxHealth()}";
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync(1);
        }

        public void ToMenu()
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}
