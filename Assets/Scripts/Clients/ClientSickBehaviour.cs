using System.Collections;
using UnityEngine;

public class ClientSickBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _vomitPrefab;

    public IEnumerator SickBehaviour()
    {
        yield return new WaitForSeconds(2f);

        Vector3 pos = transform.position;
        pos.y = 0.01f;

        Instantiate(_vomitPrefab, pos, Quaternion.identity);
    }
}
