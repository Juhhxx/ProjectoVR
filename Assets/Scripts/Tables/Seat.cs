using NaughtyAttributes;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] private Vector3 _seatingOffSet;
    [SerializeField] private int _seatingDirection;
    [SerializeField] private bool _drawGizmos;

    [field: SerializeField, ReadOnly] public bool IsOccupied { get; private set; }

    public void OccupySeat() => IsOccupied = true;
    public void UnoccupySeat() => IsOccupied = false;
    public (Vector3, int) GetSeatingPositionDirection() => (transform.position + _seatingOffSet, _seatingDirection);

    private void OnDrawGizmos()
    {
        // Drawn seating pivot
        Gizmos.color = Color.green;

        Vector3 seatPos = transform.position + _seatingOffSet;

        Gizmos.DrawLine(seatPos + Vector3.down, seatPos + Vector3.up);
        Gizmos.DrawLine(seatPos + Vector3.left, seatPos + Vector3.right);
        Gizmos.DrawLine(seatPos + Vector3.back, seatPos + Vector3.forward);

        // Drawn seating direction
        Gizmos.color = Color.blue;

        Vector3 direction = Quaternion.Euler(0, _seatingDirection, 0) * Vector3.forward;

        Gizmos.DrawLine(seatPos, seatPos + direction);
    }
}
