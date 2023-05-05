using UnityEngine;

using Items;



namespace Shooting.Weapon
{
    public class Weapon_Tadpole : MonoBehaviour, IWeapon
    {
        private const float ShootForce = 600;

        private readonly Tadpole_Controller _tadpoleController;

        public Weapon_Tadpole(Tadpole_Controller tadpole)
        {
            _tadpoleController = tadpole;
        }

        public void Shoot(Vector2 mousePosition, OnRanOutOfAmmunition callback)
        {
            _tadpoleController.Shoot();

            Rigidbody2D projectile = _tadpoleController.GetComponent<Rigidbody2D>();

            Vector2 modifiedPosition = new Vector2(mousePosition.x * Random.Range(0.95f, 1.05f), mousePosition.y * Random.Range(0.95f, 1.05f));
            Vector2 targetVector = (Camera.main.ScreenToWorldPoint(modifiedPosition) - _tadpoleController.transform.position);

            float forceModifier = Random.Range(0.8f, 1.3f);
            projectile.AddForce(targetVector.normalized * ShootForce * forceModifier);

            callback.Invoke();
        }
    }
}