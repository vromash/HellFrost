using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dice
{
    [Serializable]
    public struct DiceAbilityToSprite
    {
        public int value;
        public Sprite sprite;
    }

    public class DiceUI : MonoBehaviour
    {
        [SerializeField] private Sprite[] d4;
        [SerializeField] private Sprite[] d6;
        [SerializeField] private Sprite[] d8;
        [SerializeField] private Sprite[] d10;
        [SerializeField] private Sprite[] d12;
        [SerializeField] private Sprite[] d20;
        [SerializeField] private DiceAbilityToSprite[] abilities;

        [SerializeField] private Animator kettle;
        [SerializeField] private Image bonus;
        [SerializeField] private Image dice;

        private int _damage;
        private readonly Dictionary<int, Sprite> _abilityValueToSprite = new();

        private void Start()
        {
            foreach (var a in abilities)
            {
                // Debug.Log(a.value + ": " + a.sprite.name);
                _abilityValueToSprite.Add(a.value, a.sprite);
            }
        }

        public void Initialize(int damage, DiceShape shape)
        {
            _damage = damage;
            var sprite = shape switch
            {
                DiceShape.Four => d4[damage - 1],
                DiceShape.Six => d6[damage - 1],
                DiceShape.Eight => d8[damage - 1],
                DiceShape.Ten => d10[damage - 1],
                DiceShape.Twelve => d12[damage - 1],
                DiceShape.Twenty => d20[damage - 1],
                _ => d4[damage - 1]
            };

            dice.sprite = sprite;
            dice.SetNativeSize();
            // dice.GetComponent<RectTransform>().sizeDelta = new Vector2(sprite.texture.width, sprite.texture.height);
        }

        public void EnableKettle()
        {
            kettle.gameObject.SetActive(true);
            if (_abilityValueToSprite.ContainsKey(_damage))
            {
                FireKettle();
                EnableBonus();
            }
        }

        private void FireKettle()
        {
            kettle.SetTrigger("Fire");
        }

        private void EnableBonus()
        {
            bonus.gameObject.SetActive(true);
            bonus.sprite = _abilityValueToSprite[_damage];
        }
    }
}
