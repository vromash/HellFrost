using System;
using Dice;
using Hero;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        public event Action<Vector2, int, FontColor> onHit;

        [SerializeField] private DiceShape damageDice;

        private int _maxDamage;
        private bool _canHit = true;
        private float _hitCooldown;

        private void Awake()
        {
            _maxDamage = damageDice switch
            {
                DiceShape.Four => 4,
                DiceShape.Six => 6,
                DiceShape.Eight => 8,
                DiceShape.Ten => 10,
                DiceShape.Twelve => 12,
                DiceShape.Twenty => 20,
                _ => 4
            };
        }

        private void Update()
        {
            if (_canHit) return;

            _hitCooldown -= Time.deltaTime;
            if (_hitCooldown <= 0)
                _canHit = true;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Hero") && _canHit)
            {
                Hit(col.gameObject);
            }
        }

        private void OnCollisionStay2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Hero") && _canHit)
            {
                Hit(col.gameObject);
            }
        }

        private void Hit(GameObject go)
        {
            var dmg = Damage();
            go.GetComponent<HeroHealth>().TakeDamage(dmg);
            onHit?.Invoke(transform.position, dmg, FontColor.Red);
            _canHit = false;
            _hitCooldown = 0.5f;
        }

        private int Damage() => Random.Range(1, _maxDamage);
    }
}
