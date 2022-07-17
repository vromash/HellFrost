using Dice;
using UnityEngine;

namespace Hero
{
    public enum ShieldType
    {
        All,
        Fire,
        Frost
    }

    public class HeroShield : MonoBehaviour
    {
        [SerializeField] private DiceTray diceTray;
        [SerializeField] private ParticleSystem shieldFire;
        [SerializeField] private ParticleSystem shieldFrost;
        [SerializeField] private ParticleSystem shieldAll;
        [SerializeField] private HeroMovement heroMovement;

        private CircleCollider2D _collider2D;
        private ShieldType _currentType;
        private bool _isActive;

        private void Awake()
        {
            _collider2D = GetComponent<CircleCollider2D>();
        }

        private void Start()
        {
            PrepareShields();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !_isActive)
            {
                var dice = diceTray.Get();
                heroMovement.UpdateDiceElement(diceTray.Next().Element());
                Activate(dice.Element() == DiceElement.Fire ? ShieldType.Fire : ShieldType.Frost);
            }
        }

        private void Activate(ShieldType t)
        {
            _isActive = true;
            _collider2D.enabled = true;
            _currentType = t;
            switch (t)
            {
                case ShieldType.All:
                    shieldAll.Play();
                    break;
                case ShieldType.Fire:
                    shieldFire.Play();
                    break;
                case ShieldType.Frost:
                    shieldFrost.Play();
                    break;
                default:
                    shieldFire.Play();
                    break;
            }
        }

        private void PrepareShields()
        {
            _collider2D.enabled = false;
            shieldAll.GetComponent<ShieldCallback>().onParticleEnd += DisableShield;
            shieldFire.GetComponent<ShieldCallback>().onParticleEnd += DisableShield;
            shieldFrost.GetComponent<ShieldCallback>().onParticleEnd += DisableShield;
        }

        private void DisableShield()
        {
            _isActive = false;
            _collider2D.enabled = false;
        }

        public bool CanPass(bool isEven)
        {
            switch (_currentType)
            {
                case ShieldType.All:
                    return false;
                case ShieldType.Fire when !isEven:
                case ShieldType.Frost when isEven:
                    return true;
                default:
                    return false;
            }
        }

        public bool ShouldBounce() => _currentType == ShieldType.All;
    }
}
