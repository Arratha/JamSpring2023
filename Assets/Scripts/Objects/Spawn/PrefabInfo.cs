using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



namespace Objects.Spawner
{
    [ExecuteInEditMode]
    public class PrefabInfo : MonoBehaviour
    {
        [HideInInspector] public GameObject Prefab;

#if UNITY_EDITOR
        private void Awake()
        {
            Prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject);
        }
#endif
    }
}