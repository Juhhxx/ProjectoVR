using System.Collections;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _clientPrefab;

    [SerializeField, Range (0,10)] private float _spawningInterval;

    private void Start()
    {
        StartCoroutine(SpawnClientsCR());
    }

    private IEnumerator SpawnClientsCR()
    {
        while (true)
        {
            Instantiate(_clientPrefab, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(_spawningInterval);
        }
    }
}
