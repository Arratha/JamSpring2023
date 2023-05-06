using UnityEngine;
using Shooting;


namespace Quest
{
    public class SunFlowerQuest_Controller : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider1;
        [SerializeField] private Collider2D _collider2;
        [SerializeField] private Animator anim;

        private IShootable _shootable;

        private void Awake()
        {
            _shootable = GetComponentInChildren<IShootable>();

            _shootable.OnShooted += Shooted;
        }

        private void Shooted(ProjectileType type, Vector2 projectilePosition, OnShootCallback callback)
        {
            Debug.Log("Shoot");

            if (type != ProjectileType.DropOfWater)
                return;

            if (type == ProjectileType.DropOfWater)
            {
                _collider1.isTrigger = false;
                _collider2.isTrigger = false;
                anim.Play("SunFlower_Rise");
            }
            
        }

        private void OnDestroy()
        {
            _shootable.OnShooted -= Shooted;
        }
    }
}