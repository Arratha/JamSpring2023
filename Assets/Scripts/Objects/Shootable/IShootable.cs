using System;

using UnityEngine;


namespace Shooting
{
    public enum ProjectileType { DropOfWater, Knife, Firefly, Tadpole }
    public delegate void OnShootCallback();

    public interface IShootable
    {
        public void Shoot(ProjectileType type, Vector2 projectilePosition, OnShootCallback callback = null);
        public event Action<ProjectileType, Vector2, OnShootCallback?> OnShooted;
        public GameObject gameObject { get; }
    }
}