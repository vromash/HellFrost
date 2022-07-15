using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dice
{
    public class DiceTray : MonoBehaviour
    {
        [SerializeField] private int size;
        [SerializeField] private GameObject uiDicePrefab;
        [SerializeField] private Transform uiDiceParent;

        private readonly Queue<Dice> _tray = new();
        private readonly Queue<GameObject> _displays = new();

        private void Start()
        {
            for (var i = 0; i < size; i++)
                AddDice();
        }

        public Dice Get()
        {
            var dice = _tray.Dequeue();
            var display = _displays.Dequeue();
            Destroy(display);
            AddDice();
            return dice;
        }

        private void AddDice()
        {
            var dice = new Dice();
            _tray.Enqueue(dice);

            var diceDisplay = Instantiate(uiDicePrefab, uiDiceParent);
            diceDisplay.GetComponent<TMP_Text>().text = $"{dice.Value()} {dice.Shape()}";
            _displays.Enqueue(diceDisplay);
        }
    }
}
