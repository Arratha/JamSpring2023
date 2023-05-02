using UnityEngine;

using Settings.Tags;


namespace Shooting
{
    public class ShootingProjectile_Drop : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag(Tags.DropTag)
                && !collision.gameObject.CompareTag(Tags.ProjectileTag))
            {
                if (collision.TryGetComponent(out IShootable target))
                    target.Shoot();

                Destroy(gameObject);
            }
        }
    }
}