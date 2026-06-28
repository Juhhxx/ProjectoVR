using System;
using UnityEngine;
using UnityEngine.AI;

public class ClientMovement : MonoBehaviour
{
    [SerializeField] private float _onArriveDistance;
    private NavMeshAgent _agent;
    public event Action OnArrive;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void ToggleAgent(bool onOff) => _agent.enabled = onOff;

    public void SetDestination(Vector3 pos, Action onArrive = null)
    {
        _agent.SetDestination(pos);
        OnArrive = onArrive;
    }

    private bool CheckOnArrive()
    {
        return Vector3.Distance(_agent.destination, transform.position) <= _onArriveDistance;
    }

    private void Update()
    {
        if (CheckOnArrive())
        {
            OnArrive?.Invoke();
            Debug.Log($"Client {name} arrived at {_agent.destination}");
        }
    }
}
