using System.Collections;
using NaughtyAttributes;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ClientWanderBehaviour : MonoBehaviour
{
    [SerializeField, MinMaxSlider(5,15)] private Vector2 _minMaxWanderRadius;
    [SerializeField] private float _waitTime = 2f;
    private ClientMovement _clientMovement;

    private void Awake()
    {
        _clientMovement = GetComponent<ClientMovement>();
    }

    private Vector3 ChoosePointInNavmesh()
    {
        Vector2 dir = Random.insideUnitCircle;
        float distance = Random.Range(_minMaxWanderRadius.x, _minMaxWanderRadius.y);

        Vector3 candidate = transform.position + new Vector3(dir.x, 0f, dir.y) * distance;

        if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else return ChoosePointInNavmesh();
    }

    public IEnumerator WanderBehaviourCR()
    {
        Vector3 point = ChoosePointInNavmesh();

        bool arrived = false;

        _clientMovement.SetDestination(point, () => arrived = true);

        yield return new WaitUntil(() => arrived);

        yield return new WaitForSeconds(_waitTime);
    }
}
