using UnityEngine;

using TMPro;


namespace Shooting
{
    [RequireComponent(typeof(Collider2D))]
    public class TestShootable : MonoBehaviour, IShootable
    {
        [SerializeField] private TextMeshProUGUI _textField;

        private string[] _quotes = new string[] {
            "�������, ����� ����������",
            "���������� �����",
            "� ���, ���� ��������",
            "���, � ���� ������",
            "��� ���� ��������� �����!!!",
            "��� �������� ���� ���� ���������",
            "��� �������, ���� ���� �������",
            "���� ��������� ����� ��������� ���",
            "��, ������� ���� ������..."
        };

        public void Shoot()
        {
            _textField.text = _quotes[Random.Range(0, _quotes.Length)];
        }
    }
}