using System.Collections;
using UnityEngine;

public class ClientBrain : MonoBehaviour
{
    public enum ClientStates { LookingForTable, LookingForSeat, Seating, Eating, Trash, Leaving, Wander, Sick, Die }

    [SerializeField] private ClientStates _startingState = ClientStates.LookingForTable;
    [SerializeField] private float _sickChance = 0.3f;

    private ClientStates _currentState;

    private ClientMovement _clientMovement;
    private ClientWanderBehaviour _clientWander;
    private ClientEatingBehaviour _clientEating;
    private ClientTrashBehaviour _clientTrash;
    private ClientSickBehaviour _clientSick;

    private TablesManager _tablesManager;
    private Table _currentTable = null;
    private bool _noTables = false;
    private Seat _currentSeat = null;

    private TrashGenerator _trashGenerator;

    private Vector3 _entrance;
    private bool _alive = true;
    private bool _alreadyAte = false;

    private void Start()
    {
        _tablesManager = FindAnyObjectByType<TablesManager>();
        _trashGenerator = FindAnyObjectByType<TrashGenerator>();

        _clientMovement = GetComponent<ClientMovement>();
        _clientWander = GetComponent<ClientWanderBehaviour>();
        _clientEating = GetComponent<ClientEatingBehaviour>();
        _clientTrash = GetComponent<ClientTrashBehaviour>();
        _clientSick = GetComponent<ClientSickBehaviour>();

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

        yield return new WaitForSeconds(0.5f);

        (Vector3 pos, int rot) = _currentSeat.GetSeatingPositionDirection();

        transform.position = pos;
        transform.rotation = Quaternion.Euler(0f, rot, 0f);

        _tablesManager.ReleaseTable(_currentTable);
        
        yield return null;

        _currentState = ClientStates.Eating;
    }

    private IEnumerator Eat()
    {
        yield return _clientEating.EatingBehaviour();

        _alreadyAte = true;

        float rnd = Random.Range(0f, 1f);

        if (rnd <= _sickChance) _currentState = ClientStates.Sick;
        else _currentState = ClientStates.Trash; 
    }

    private IEnumerator Sick()
    {
        _clientMovement.ToggleAgent(true);
        _currentSeat.UnoccupySeat();

        _currentTable = null;
        _currentSeat = null;
        
        yield return _clientWander.WanderBehaviourCR();

        yield return _clientSick.SickBehaviour();

        _currentState = ClientStates.Leaving;
    }

    private IEnumerator Trash()
    {
        _clientMovement.ToggleAgent(true);
        _currentSeat.UnoccupySeat();

        _currentTable = null;
        _currentSeat = null;

        TrashPoint trashPoint = _trashGenerator.GetAvailableTrashPoint();

        if (trashPoint == null)
        {
            _currentState = ClientStates.Wander;
            yield break;
        }

        bool arrived = false;

        _clientMovement.SetDestination(trashPoint.Point.position, () => arrived = true);

        yield return new WaitUntil(() => arrived);

        yield return _clientTrash.TrashBehaviour(_trashGenerator, trashPoint);

        _currentState = ClientStates.Leaving;
    }

    private IEnumerator Leave()
    {
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

    private IEnumerator Wander(ClientStates nextState = ClientStates.LookingForTable)
    {
        _noTables = false;

        yield return _clientWander.WanderBehaviourCR();

        _currentState = nextState;
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
                
                case ClientStates.Sick:
                    yield return Sick();
                    break;
                
                case ClientStates.Trash:
                    yield return Trash();
                    break;
                
                case ClientStates.Leaving:
                    yield return Leave();
                    break;
                
                case ClientStates.Die:
                    Die();
                    break;
                
                case ClientStates.Wander:
                    if (_alreadyAte) yield return Wander(ClientStates.Trash);
                    else yield return Wander();
                    break;
                
                default:
                    yield return null;
                    break;
            }
        }
    }
}
