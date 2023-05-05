using System;

using UnityEngine;


namespace Shooting
{
    public class Shootable : MonoBehaviour, IShootable
    {
        public event Action<ProjectileType, Vector2, OnShootCallback> OnShooted;

        public void Shoot(ProjectileType type, Vector2 projectilePosition, OnShootCallback callback = null)
        {
            OnShooted?.Invoke(type, projectilePosition, callback);
        }
    }
}