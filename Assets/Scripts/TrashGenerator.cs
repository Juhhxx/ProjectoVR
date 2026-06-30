using System.Linq;
using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    [SerializeField] private TrashDataBase _trashDataBase;
    [SerializeField] private float _trashSpawnRadius = 2f;

    [SerializeField] private TrashPoint[] _trashPoints;

    public TrashPoint GetAvailableTrashPoint()
    {
        var points = _trashPoints.Where(p => !p.IsOccupied).ToArray();

        return points.Length > 0 ? points[Random.Range(0, points.Length)] : null;
    }
    public void GenerateTrash(TrashPoint trashPoint)
    {
        if (trashPoint == null) return;

        Instantiate(_trashDataBase.GetTrashPrefabFromType(ChooseType()),
                    ChoosePosition(trashPoint.TrashSpawn.position),
                    Quaternion.identity);
    }

    private Vector3 ChoosePosition(Vector3 origin)
    {
        Vector3 dir = Random.insideUnitCircle;
        float dist = Random.Range(0, _trashSpawnRadius);

        return origin + (dir * dist);
    }

    private TrashType ChooseType()
    {
        TrashType type = (TrashType)Random.Range(0, 3);

        return type;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        foreach (TrashPoint tp in _trashPoints)
        {
            Gizmos.DrawWireSphere(tp.TrashSpawn.position, _trashSpawnRadius);
        }
    }
}

[System.Serializable]
public class TrashPoint
{
    public Transform Point;
    public bool IsOccupied;
    public Transform TrashSpawn;
}