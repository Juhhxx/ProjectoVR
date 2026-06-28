using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private List<Transform> _tablePivots;
    [SerializeField, ReadOnly] private int _numberOfSeats;
    public int NumberOfSeats => _numberOfSeats;

    [SerializeField, ReadOnly] private List<Seat> _seats;
    public IList<Seat> Seats => _seats;

    public bool IsReserved { get; set; }

    public bool HasSpace() => _seats.Any((seat) => !seat.IsOccupied);
    public int GetNumberOfFreeSeats() => _seats.Count(seat => !seat.IsOccupied);
    public Seat GetFreeSeat() => _seats.Where((seat) => !seat.IsOccupied).ToList()[Random.Range(0, GetNumberOfFreeSeats())];
    public Transform GetTableWaitingPivot() => _tablePivots[Random.Range(0, _tablePivots.Count)];

    private void Start()
    {
        _seats = GetComponentsInChildren<Seat>().ToList();
        _numberOfSeats = _seats.Count;
    }
}
