using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Objects.Spawner
{
    [ExecuteInEditMode]
    public class PrefabInfo : MonoBehaviour
    {
        [SerializeField] public GameObject Prefab;

#if UNITY_EDITOR
        private void Awake()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode && transform.parent != null && Prefab == null)
                Prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(transform.parent.gameObject);
        }
#endif
    }
}