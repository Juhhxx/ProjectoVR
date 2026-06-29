using System.Linq;
using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _trashPrefab;

    [SerializeField] private TrashPoint[] _trashPoints;

    public TrashPoint GetAvailableTrashPoint()
    {
        var points = _trashPoints.Where(p => !p.IsOccupied).ToArray();

        return points.Length > 0 ? points[Random.Range(0, points.Length)] : null;
    }
    public void GenerateTrash(TrashPoint trashPoint)
    {
        if (trashPoint == null) return;

        Instantiate(_trashPrefab, trashPoint.TrashSpawn.position, Quaternion.identity);
    }
}

[System.Serializable]
public class TrashPoint
{
    public Transform Point;
    public bool IsOccupied;
    public Transform TrashSpawn;
}