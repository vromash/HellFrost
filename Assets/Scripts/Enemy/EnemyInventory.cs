using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyInventory : MonoBehaviour
    {
        public event Action<int> onCoinsGived;

        [SerializeField] private int coins;

        public void GiveCoins()
        {
            onCoinsGived?.Invoke(coins);
        }
    }
}
