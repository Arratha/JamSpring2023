using UnityEngine;

using Settings.Tags;

using Shooting;


namespace Drop.Shooting
{
    public class Drop_ShootingProjectile : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag(Tags.DropTag))
            {
                if (collision.TryGetComponent(out IShootable target))
                    target.Shoot();

                Destroy(gameObject);
            }
        }
    }
}