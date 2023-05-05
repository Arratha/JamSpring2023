using System;
using System.Collections.Generic;

using UnityEngine;

using TMPro;

using Drop;
using Settings.Tags;

using Random = UnityEngine.Random;


namespace Shooting
{
    [RequireComponent(typeof(Collider2D))]
    public class TestTarget : MonoBehaviour, IShootable
    {
        [SerializeField] private TextMeshProUGUI _textField;

        public event Action<ProjectileType, Vector2, OnShootCallback> OnShooted;

        private Dictionary<ProjectileType, string[]> _quotes = new Dictionary<ProjectileType, string[]>
        {
            {
                ProjectileType.DropOfWater, new string[]
                {
                    " ажетс€, дождь начинаетс€",
                    "“епленька€ пошла",
                    "ќ нет, мен€ замочили",
                    "„ел, € типа кактус",
                    "Ё“ќ ЅџЋј ѕќ—Ћ≈ƒЌяя  јѕЋя!!!",
                    "Ёто подмочит лишь твою репутацию",
                    "ћне кажетс€, этот билд сџрќвјт",
                    "Ќаши отношени€ скоро достигнут дна",
                    "Ёх, сколько воды утекло..."
                }
            },
            {
                ProjectileType.Knife, new string[]
                {
                    "ѕќЋќ∆» Ќќ∆!",
                    "я тоже кое-что знаю о режуще- оЋюўи’ предметах",
                    "Ёто единственна€ острота, что пришла тебе на ум?"
                }
            },
            {
                ProjectileType.Firefly, new string[]
                {
                    "—ерьезно? —ветл€чок?",
                    "ќ, € просветлен",
                    "«наешь, € не фотопленка, мне не страшно"
                }
            }
        };


        public void Shoot(ProjectileType type, Vector2 projectilePosition, OnShootCallback callback = null)
        {
            _textField.text = _quotes[type][Random.Range(0, _quotes[type].Length - 1)];

            callback?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.DropTag))
            {
                Drop_Controller drop = collision.GetComponentInParent<Drop_Controller>();
                Collider2D collider = GetComponent<Collider2D>();
                Vector2 position = new Vector2(collider.bounds.center.x, collider.bounds.min.y);

                drop?.DropCondition.DoDamage(position);
            }
        }
    }
}