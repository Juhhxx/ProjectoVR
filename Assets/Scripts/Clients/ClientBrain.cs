using System.Collections;
using UnityEngine;

public class ClientBrain : MonoBehaviour
{
    public enum ClientStates { LookingForTable, LookingForSeat, Seating, Eating, Leaving, Wander, Die }

    [SerializeField] private ClientStates _startingState = ClientStates.LookingForTable;

    private ClientStates _currentState;

    private ClientMovement _clientMovement;
    private ClientWanderBehaviour _clientWander;
    private ClientEatingBehaviour _clientEating;

    private TablesManager _tablesManager;
    private Table _currentTable = null;
    private bool _noTables = false;
    private Seat _currentSeat = null;

    private Vector3 _entrance;
    private bool _alive = true;

    private void Start()
    {
        _tablesManager = FindAnyObjectByType<TablesManager>();

        _clientMovement = GetComponent<ClientMovement>();
        _clientWander = GetComponent<ClientWanderBehaviour>();
        _clientEating = GetComponent<ClientEatingBehaviour>();

        _currentState = _startingState;

        _entrance = transform.position;

        StartCoroutine(StateMachineCR());
    }

    // Looking for table
    public void SetTable(Table table)
    {
        _currentTable = table;
    }
    public void NoTables()
    {
        _noTables = true;
    }

    private void ChooseStayAtTable()
    {
        bool result = Random.Range(0, _currentTable.NumberOfSeats) <= _currentTable.GetNumberOfFreeSeats();

        if (result) _currentState = ClientStates.LookingForSeat;
    }

    private IEnumerator LookForTable()
    {
        _clientMovement.ToggleAgent(true);

        _tablesManager.RequestTable(this);

        yield return new WaitUntil(() => _currentTable != null || _noTables);

        if (_noTables)
        {
            _currentState = ClientStates.Wander;
            yield break;
        }

        bool arrivedAtTable = false;
        
        _clientMovement.SetDestination(_currentTable.GetTableWaitingPivot().position,
                                                            () => arrivedAtTable = true);
        
        yield return new WaitUntil(() => arrivedAtTable);

        ChooseStayAtTable();

        yield return null;
    }

    // Lookign for seat
    private IEnumerator LookForSeat()
    {
        _currentSeat = _currentTable.GetFreeSeat();
        
        _tablesManager.ReleaseTable(_currentTable);

        _currentState = ClientStates.Seating;

        yield return null;
    }

    private IEnumerator Seat()
    {
        _clientMovement.ToggleAgent(false);

        _currentSeat.OccupySeat();

        (Vector3 pos, int rot) = _currentSeat.GetSeatingPositionDirection();

        transform.position = pos;
        transform.rotation = Quaternion.Euler(0f, rot, 0f);

        _tablesManager.ReleaseTable(_currentTable);
        
        yield return null;

        _currentState = ClientStates.Eating;
    }

    private IEnumerator Eat()
    {
        yield return _clientEating.EatingBehaviour(_currentTable);

        _currentState = ClientStates.Leaving; 
    }

    private IEnumerator Leave()
    {
        _clientMovement.ToggleAgent(true);
        _currentSeat.UnoccupySeat();

        _currentTable = null;
        _currentSeat = null;

        bool arrivedExit = false;

        _clientMovement.SetDestination(_entrance, () => arrivedExit = true);

        yield return new WaitUntil(() => arrivedExit);

        _currentState = ClientStates.Die;
    }

    private void Die()
    {
        Debug.Log("KILL YOURSELF");

        // Temp
        gameObject.SetActive(false);

        _alive = false;
    }

    private IEnumerator Wander()
    {
        _noTables = false;

        yield return _clientWander.WanderBehaviourCR();

        _currentState = ClientStates.LookingForTable;
    }

    public IEnumerator StateMachineCR()
    {
        while (_alive)
        {
            Debug.Log($"IN STATE {_currentState}");

            switch (_currentState)
            {
                case ClientStates.LookingForTable:
                    yield return LookForTable();
                    break;
                
                case ClientStates.LookingForSeat:
                    yield return LookForSeat();
                    break;

                case ClientStates.Seating:
                    yield return Seat();
                    break;

                case ClientStates.Eating:
                    yield return Eat();
                    break;
                
                case ClientStates.Leaving:
                    yield return Leave();
                    break;
                
                case ClientStates.Die:
                    Die();
                    break;
                
                case ClientStates.Wander:
                    yield return Wander();
                    break;
                
                default:
                    yield return null;
                    break;
            }
        }
    }
}
