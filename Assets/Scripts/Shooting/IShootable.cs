using UnityEngine;


namespace Shooting
{
    public enum ProjectileType { DropOfWater, Knife, Firefly }
    public delegate void OnShootCallback();

    public interface IShootable
    {
        public void Shoot(ProjectileType type, OnShootCallback callback = null);
        public GameObject gameObject { get; }
    }
}