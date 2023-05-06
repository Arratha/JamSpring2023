using System;

using UnityEngine;


namespace Objects.Spawner
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private bool _shouldSaveTransform;
        [SerializeField] private GameObject _objectToRespawn;

        private ObjectInfo? _storedObject;

        private void Awake()
        {
            if (_objectToRespawn == null)
                return;

            if (_shouldSaveTransform)
                _storedObject = new ObjectInfo(_objectToRespawn);
            else
                _storedObject = new ObjectInfo(_objectToRespawn, transform.position);
        }

        private void FixedUpdate()
        {
            if (_objectToRespawn == null && _storedObject != null)
                _objectToRespawn = ((ObjectInfo)_storedObject).Respawn();
        }

        private struct ObjectInfo
        {
            private GameObject _objectPrefab;

            private Vector2 _position;
            private Quaternion _quaternion;

            public ObjectInfo(GameObject objectToRespawn)
            {
                _objectPrefab = null;

                if (objectToRespawn.TryGetComponent(out PrefabInfo info))
                    _objectPrefab = info.Prefab;
                else
                    throw new Exception($"Has no {typeof(PrefabInfo)}: { objectToRespawn }");

                _position = objectToRespawn.transform.position;
                _quaternion = objectToRespawn.transform.rotation;
            }

            public ObjectInfo(GameObject objectToRespawn, Vector2 position)
            {
                _objectPrefab = null;

                if (objectToRespawn.TryGetComponent(out PrefabInfo info))
                    _objectPrefab = info.Prefab;
                else
                    throw new Exception($"Has no {typeof(PrefabInfo)}: { objectToRespawn }");

                _position = position;
                _quaternion = new Quaternion(0, 0, 0, 0);
            }

            public GameObject Respawn()
            {
                return Instantiate(_objectPrefab, _position, _quaternion);
            }
        }
    }
}