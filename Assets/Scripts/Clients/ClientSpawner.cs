using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _clientPrefab;

    [SerializeField, MinMaxSlider(0,15)] private Vector2 _spawningInterval;

    public void StartSpawning()
    {
        StartCoroutine(SpawnClientsCR());
    }

    private IEnumerator SpawnClientsCR()
    {
        while (true)
        {
            Instantiate(_clientPrefab, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(_spawningInterval.x, _spawningInterval.y));
        }
    }
}
