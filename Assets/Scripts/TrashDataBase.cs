using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TrashDataBase", menuName = "Scriptable Objects/TrashDataBase")]
public class TrashDataBase : ScriptableObject
{
    [System.Serializable]
    public struct TrashTypePrefabs
    {
        public TrashType Type;
        public List<GameObject> Prefabs; 
    }
    
    [SerializeField] private List<TrashTypePrefabs> _trashPrefabs;

    public GameObject GetTrashPrefabFromType(TrashType type)
    {
        var prefabList = _trashPrefabs.Find((trash) => trash.Type == type).Prefabs;

        return prefabList[Random.Range(0, prefabList.Count)];
    }
}
