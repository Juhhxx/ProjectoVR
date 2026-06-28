using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _trashPrefab;

    public void GenerateTrash()
    {
        Instantiate(_trashPrefab, transform.position, Quaternion.identity);
    }
}
