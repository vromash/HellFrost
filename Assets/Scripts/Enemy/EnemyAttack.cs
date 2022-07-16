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
        public event Func<Vector2, int, GameObject> onThrew;

        [SerializeField] private DiceShape damageDice;
        [SerializeField] private bool canThrow;

        private int _maxDamage;
        private bool _allowedToAttack = true;
        private float _attackCooldown;
        private Transform _target;
        private EnemyVisuals _visuals;

        private void Awake()
        {
            _visuals = GetComponent<EnemyVisuals>();
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

        public void Initialize(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            if (_allowedToAttack)
            {
                if (!canThrow) return;
                Throw();
            }

            _attackCooldown -= Time.deltaTime;
            if (_attackCooldown <= 0)
                _allowedToAttack = true;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Hero") && _allowedToAttack)
            {
                Hit(col.gameObject);
            }
        }

        private void OnCollisionStay2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Hero") && _allowedToAttack)
            {
                Hit(col.gameObject);
            }
        }

        private void Hit(GameObject go)
        {
            _allowedToAttack = false;
            _attackCooldown = 0.5f;

            var dmg = Damage();
            go.GetComponent<HeroHealth>().TakeDamage(dmg);
            onHit?.Invoke(transform.position, dmg, FontColor.Red);
        }

        private void Throw()
        {
            _allowedToAttack = false;
            _attackCooldown = 2f;

            var diceGO = onThrew?.Invoke(transform.position, Damage());
            diceGO.GetComponent<DiceProjectile>().Throw(_target.position);
            _visuals.SetThrowAnimation();

        }

        private int Damage() => Random.Range(1, _maxDamage);
    }
}
