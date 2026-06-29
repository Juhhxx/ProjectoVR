using UnityEngine;
using UnityEngine.Events;

public class Trashcan : MonoBehaviour
{
    [SerializeField] private TrashType _trashcanType;
    
    public UnityEvent OnCorrectTrash;
    public UnityEvent OnWrongTrash;

    private void OnTriggerEnter(Collider other)
    {
        Trash trash = other.GetComponent<Trash>();

        if (trash != null)
        {
            if (trash.Type == _trashcanType) OnCorrectTrash?.Invoke();
            else OnWrongTrash?.Invoke();
            
            Destroy(other.gameObject);
        }
    }
}
