using UnityEngine;
using NaughtyAttributes;
using System.Collections;

public class ClientTrashBehaviour : MonoBehaviour
{
    [SerializeField, MinMaxSlider(0, 5)] private Vector2Int _minMaxTrash;


    public IEnumerator TrashBehaviour(TrashGenerator trashGenerator, TrashPoint trashPoint)
    {
        trashPoint.IsOccupied = true;

        int trashProduced = Random.Range(_minMaxTrash.x, _minMaxTrash.y + 1);

        for (int i = 0; i < trashProduced; i++)
        {
            trashGenerator.GenerateTrash(trashPoint);
            yield return new WaitForSeconds(0.5f);
        }

        trashPoint.IsOccupied = false;
    }
}
