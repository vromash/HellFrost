using System;

namespace Dice
{
    public enum DiceShape
    {
        Four,
        Six,
        Eight,
        Ten,
        Twelve,
        Twenty
    }

    public enum DiceElement
    {
        Fire,
        Frost
    }

    public enum DiceAbility
    {
        Fireball,
        Shield,
        Wave,
        Ricochet,
        Freeze,
        Heal,
        Dash,
        Ghost,
        Shotgun,
        Lightning,
    }

    public class Dice
    {
        private readonly Random _rnd;

        private int _value;
        private readonly DiceElement _diceElement;
        private readonly DiceShape _shape;

        private readonly DiceShape[] _shapes =
            {DiceShape.Four, DiceShape.Six, DiceShape.Eight, DiceShape.Ten, DiceShape.Twelve, DiceShape.Twenty};

        public Dice()
        {
            _rnd = new Random();
            _value = _rnd.Next(1, 20);
            _shape = Shape(_value);
            _diceElement = _value % 2 == 0 ? DiceElement.Fire : DiceElement.Frost;
        }

        public int Value() => _value;
        public DiceElement Element() => _diceElement;
        public DiceShape Shape() => _shape;

        public void IncreaseDamage() => _value++;

        public bool HasAbility()
        {
            switch (_value)
            {
                case 1:
                case 3:
                case 4:
                case 5:
                case 6:
                case 8:
                case 10:
                case 11:
                case 12:
                case 20:
                    return true;
                default:
                    return false;
            }
        }

        public DiceAbility Ability()
        {
            switch (_value)
            {
                case 1:
                    return DiceAbility.Fireball;
                case 3:
                    return DiceAbility.Shield;
                case 4:
                    return DiceAbility.Wave;
                case 5:
                    return DiceAbility.Ricochet;
                case 6:
                    return DiceAbility.Freeze;
                case 8:
                    return DiceAbility.Heal;
                case 10:
                    return DiceAbility.Dash;
                case 11:
                    return DiceAbility.Ghost;
                case 12:
                    return DiceAbility.Shotgun;
                case 20:
                    return DiceAbility.Lightning;
                default:
                    return DiceAbility.Fireball;
            }
        }

        public int ShapeValue()
        {
            return _shape switch
            {
                DiceShape.Four => 4,
                DiceShape.Six => 6,
                DiceShape.Eight => 8,
                DiceShape.Ten => 10,
                DiceShape.Twelve => 12,
                DiceShape.Twenty => 20,
                _ => 20
            };
        }

        public string ShapeString()
        {
            return _shape switch
            {
                DiceShape.Four => "D4",
                DiceShape.Six => "D6",
                DiceShape.Eight => "D8",
                DiceShape.Ten => "D10",
                DiceShape.Twelve => "D12",
                DiceShape.Twenty => "D20",
                _ => "D20"
            };
        }

        private DiceShape Shape(int value)
        {
            return value switch
            {
                > 12 => DiceShape.Twenty,
                > 10 => _shapes[_rnd.Next(4, 6)],
                > 8 => _shapes[_rnd.Next(3, 6)],
                > 6 => _shapes[_rnd.Next(2, 6)],
                > 4 => _shapes[_rnd.Next(1, 6)],
                _ => _shapes[_rnd.Next(0, 6)]
            };
        }
    }
}
