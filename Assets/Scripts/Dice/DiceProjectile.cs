using System;
using Enemy;
using Hero;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dice
{
    public class DiceProjectile : MonoBehaviour
    {
        public event Action<Vector2, int, FontColor> onHit;
        public event Action<GameObject> onDestroy;

        [SerializeField] private AudioClip[] audioClips;
        [SerializeField] private float speed;
        [SerializeField] private float ttl;
        [SerializeField] private float minVelocity;
        [SerializeField] private float freezeTime;

        [SerializeField]   private AudioSource _audioSourceRolls;
        [SerializeField]  private AudioSource _audioSourceResists;
        private Rigidbody2D _rb;
        private Animator _animator;
        private int _damage;
        private int _maxDamage;
        private float _lifeTimer;
        private bool _isEven;
        private bool _resisted;
        private Vector3 _lastFrameVelocity;
        private int _heroProjectileLayer;
        private int _enemyProjectileLayer;

        private bool _canFreeze;
        private bool _canRicochet;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _heroProjectileLayer = LayerMask.NameToLayer("Projectile");
            _enemyProjectileLayer = LayerMask.NameToLayer("EnemyProjectile");
        }

        private void Update()
        {
            _lastFrameVelocity = _rb.velocity;
            _lifeTimer += Time.deltaTime;
            if (_lifeTimer >= ttl)
                Destroy();
        }

        public void Initialize(int damage, int maxDamage)
        {
            _damage = damage;
            _maxDamage = maxDamage;
            _isEven = _damage % 2 == 0;
        }

        public void Throw(Vector3 targetPosition, bool withAbility = false)
        {
            SetAnimatorState();
            var position = transform.position;

            var direction = targetPosition - position;
            _rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;

            var rotation = position - targetPosition;
            var rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotationZ + 90);

            _lifeTimer = 0;
            _resisted = false;

            _audioSourceRolls.clip = audioClips[Random.Range(0, audioClips.Length)];
            _audioSourceRolls.Play();

            if (withAbility)
                EnableAbility();
        }

        private void EnableAbility()
        {
            switch (_damage)
            {
                case 5:
                    _canRicochet = true;
                    break;
                case 6:
                    _canFreeze = true;
                    break;
                default:
                    return;
            }
        }

        private void SetAnimatorState()
        {
            var element = _isEven ? "Fire" : "Frost";
            _animator.SetTrigger($"D{_maxDamage}_{element}");
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (_resisted) return;

            if (col.gameObject.CompareTag("Enemy"))
            {
                if (_canFreeze)
                {
                    Freeze(col);
                    return;
                }

                switch (_isEven)
                {
                    case true when col.gameObject.GetComponent<EnemyHealth>().ResistsEven(_damage):
                        Bounce(col.contacts[0].normal);
                        return;
                    case false when col.gameObject.GetComponent<EnemyHealth>().ResistsOdd():
                        Bounce(col.contacts[0].normal);
                        return;
                    default:
                        col.gameObject.GetComponent<EnemyHealth>().TakeDamage(_damage);
                        Hit(FontColor.White);
                        return;
                }
            }

            if (col.gameObject.CompareTag("Hero"))
            {
                col.gameObject.GetComponent<HeroHealth>().TakeDamage(_damage);
                Hit(FontColor.Red);
                return;
            }

            if (col.gameObject.CompareTag("Wall"))
            {
                Destroy();
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Shield") && gameObject.layer == _enemyProjectileLayer)
            {
                var shield = col.GetComponent<HeroShield>();
                var canPass = shield.CanPass(_isEven);
                if (canPass) return;

                if (!shield.ShouldBounce())
                {
                    Destroy();
                    return;
                }

                BounceBack();
            }
        }

        private void Freeze(Collision2D col)
        {
            _canFreeze = false;
            StartCoroutine(col.gameObject.GetComponent<EnemyHealth>().Freeze(freezeTime));
            Destroy();
        }

        private void Ricochet(Collision2D col)
        {
            _canRicochet = false;
            if (_damage <= 0)
            {
                Destroy();
                return;
            }

            BounceNotResisted(col.contacts[0].normal);
            col.gameObject.GetComponent<EnemyHealth>().TakeDamage(1);
            onHit?.Invoke(transform.position, 1, FontColor.White);
            _lifeTimer = ttl;
            _damage--;
        }

        private void BounceBack()
        {
            _rb.velocity *= -1;
            gameObject.layer = _heroProjectileLayer;

            _audioSourceResists.Play();
        }

        private void Bounce(Vector3 collisionNormal)
        {
            _resisted = true;
            _lifeTimer = ttl - 0.5f;

            var bounceSpeed = _lastFrameVelocity.magnitude;
            var direction = Vector3.Reflect(_lastFrameVelocity.normalized, collisionNormal);

            _rb.velocity = direction * Mathf.Max(bounceSpeed, minVelocity);

            _audioSourceResists.Play();
        }

        private void BounceNotResisted(Vector3 collisionNormal)
        {
            var bounceSpeed = _lastFrameVelocity.magnitude;
            var direction = Vector3.Reflect(_lastFrameVelocity.normalized, collisionNormal);

            _rb.velocity = direction * Mathf.Max(bounceSpeed, minVelocity);

            _audioSourceResists.Play();
        }

        private void Hit(FontColor color)
        {
            onHit?.Invoke(transform.position, _damage, color);
            Destroy();
        }

        private void Destroy()
        {
            onDestroy?.Invoke(gameObject);
            gameObject.SetActive(false);
        }
    }
}
