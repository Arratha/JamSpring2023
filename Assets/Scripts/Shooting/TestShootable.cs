using UnityEngine;

using TMPro;


namespace Shooting
{
    [RequireComponent(typeof(Collider2D))]
    public class TestShootable : MonoBehaviour, IShootable
    {
        [SerializeField] private TextMeshProUGUI _textField;

        private string[] _quotes = new string[] {
            " ажетс€, дождь начинаетс€",
            "“епленька€ пошла",
            "ќ нет, мен€ замочили",
            "„ел, € типа кактус",
            "Ё“ќ ЅџЋј ѕќ—Ћ≈ƒЌяя  јѕЋя!!!",
            "Ёто подмочит лишь твою репутацию",
            "ћне кажетс€, этот билд сџрќвјт",
            "Ќаши отношени€ скоро достигнут дна",
            "Ёх, сколько воды утекло..."
        };

        public void Shoot()
        {
            _textField.text = _quotes[Random.Range(0, _quotes.Length)];
        }
    }
}