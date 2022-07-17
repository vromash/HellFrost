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

    public class Dice
    {
        private readonly Random _rnd;

        private readonly int _value;
        private readonly DiceElement _diceElement;
        private readonly DiceShape _shape;

        private readonly DiceShape[] _shapes =
            {DiceShape.Four, DiceShape.Six, DiceShape.Eight, DiceShape.Ten, DiceShape.Twelve, DiceShape.Twenty};

        public Dice()
        {
            _rnd = new Random();
            _value = _rnd.Next(1, 20);
            _shape = Shape(_value);
            _diceElement = _value % 2 == 0 ? DiceElement.Frost : DiceElement.Fire;
        }

        public int Value() => _value;
        public DiceElement Element() => _diceElement;

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


        public string Shape()
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
