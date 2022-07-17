using System;
using UnityEngine;

namespace Hero
{
    public class ShieldCallback : MonoBehaviour
    {
        public event Action onParticleEnd;

        private void Awake()
        {
            var main = GetComponent<ParticleSystem>().main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        private void OnParticleSystemStopped()
        {
            onParticleEnd?.Invoke();
        }
    }
}
