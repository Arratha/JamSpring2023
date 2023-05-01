using UnityEngine;


namespace Drop.Shooting
{
    public class Drop_ShootingController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _projcetilePrefab;

        private float ShootForce = 30;

        private Transform _dropTransform;

        private void Awake()
        {
            _dropTransform = transform;
        }

        public void Shoot(Vector2 mousePosition)
        {
            Rigidbody2D projectile = Instantiate(_projcetilePrefab, _dropTransform.position, new Quaternion(0, 0, 0, 0));

            Vector2 targetVector = Camera.main.ScreenToWorldPoint(mousePosition) - _dropTransform.position;

            projectile.AddForce(targetVector.normalized * ShootForce);
        }
    }
}