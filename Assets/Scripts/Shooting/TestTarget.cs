using System.Collections.Generic;

using UnityEngine;

using TMPro;

using Drop;
using Settings.Tags;


namespace Shooting
{
    [RequireComponent(typeof(Collider2D))]
    public class TestTarget : MonoBehaviour, IShootable
    {
        [SerializeField] private TextMeshProUGUI _textField;

        private Dictionary<ProjectileType, string[]> _quotes = new Dictionary<ProjectileType, string[]>
        {
            {
                ProjectileType.DropOfWater, new string[]
                {
                    "�������, ����� ����������",
                    "���������� �����",
                    "� ���, ���� ��������",
                    "���, � ���� ������",
                    "��� ���� ��������� �����!!!",
                    "��� �������� ���� ���� ���������",
                    "��� �������, ���� ���� �������",
                    "���� ��������� ����� ��������� ���",
                    "��, ������� ���� ������..."
                }
            },
            {
                ProjectileType.Knife, new string[]
                {
                    "������ ���!",
                    "� ���� ���-��� ���� � ������-������� ���������",
                    "��� ������������ �������, ��� ������ ���� �� ��?"
                }
            },
            {
                ProjectileType.Firefly, new string[]
                {
                    "��������? ���������?",
                    "�, � ����������",
                    "������, � �� ����������, ��� �� �������"
                }
            }
        };

        public void Shoot(ProjectileType type, OnShootCallback callback = null)
        {
            _textField.text = _quotes[type][Random.Range(0, _quotes[type].Length)];

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