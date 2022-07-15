using Hero;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthDisplay;

        [SerializeField] private HeroHealth heroHealth;

        private void Start()
        {
            heroHealth.onHealthUpdated += UpdateHealthCounter;
            UpdateHealthCounter();
        }

        private void UpdateHealthCounter()
        {
            healthDisplay.text = $"HP: {heroHealth.Health()}/{heroHealth.MaxHealth()}";
        }
    }
}
