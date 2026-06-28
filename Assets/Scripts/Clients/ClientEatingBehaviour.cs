using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class ClientEatingBehaviour : MonoBehaviour
{
    [SerializeField, MinMaxSlider(2, 15)] private Vector2 _minMaxEatingTime;
    [SerializeField, MinMaxSlider(2, 15)] private Vector2Int _minMaxTrash;
    public IEnumerator EatingBehaviour(Table table)
    {
        Debug.Log($"EATING {name}");

        yield return new WaitForSeconds(Random.Range(_minMaxEatingTime.x, _minMaxEatingTime.y));

        int trashProduced = Random.Range(_minMaxTrash.x, _minMaxTrash.y);

        for (int i = 0; i < trashProduced; i++)
        {
            table.GetComponentInChildren<TrashGenerator>()?.GenerateTrash();

            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }
}
