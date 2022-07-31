using System.Collections;
using System.Collections.Generic;
using Hero;
using UnityEngine;

namespace Dice
{
    public class DiceTray : MonoBehaviour
    {
        [SerializeField] private int size;
        [SerializeField] private DiceThrower diceThrower;
        [SerializeField] private GameObject uiDicePrefab;
        [SerializeField] private Transform uiDiceParent;
        [SerializeField] private float fireballGrowSpeed;

        private readonly Queue<Dice> _tray = new();
        private readonly Queue<GameObject> _displays = new();

        private void Start()
        {
            for (var i = 0; i < size; i++)
                AddDice();
        }

        public Dice Next() => _tray.Peek();

        public Dice Get()
        {
            var dice = _tray.Dequeue();
            var display = _displays.Dequeue();
            StopAllCoroutines();
            Destroy(display);
            AddDice();
            _displays.Peek().GetComponent<DiceUI>().EnableKettle();
            return dice;
        }

        private void AddDice()
        {
            var dice = new Dice();
            _tray.Enqueue(dice);

            var diceDisplay = Instantiate(uiDicePrefab, uiDiceParent);
            diceDisplay.GetComponent<DiceUI>().Initialize(dice.Value(), dice.Shape());
            _displays.Enqueue(diceDisplay);

            if (_tray.Peek().Value() == 1)
                StartCoroutine(GrowFireball());
        }

        private IEnumerator GrowFireball()
        {
            var dice = _tray.Peek();
            var diceUI = _displays.Peek().GetComponent<DiceUI>();
            var toGrow = dice.ShapeValue() - dice.Value();
            var wait = new WaitForSeconds(fireballGrowSpeed);
            for (var i = 0; i < toGrow; i++)
            {
                yield return wait;
                dice.IncreaseDamage();
                diceUI.UpdateDamage(dice.Value());
            }

            diceThrower.Throw(false);
        }
    }
}
