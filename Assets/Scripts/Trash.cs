using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Trash : MonoBehaviour
{
    [SerializeField] private TrashType _type;
    public TrashType Type => _type;

    public static int TotalTrash = 0;

    private void Start()
    {
        TotalTrash++;
    }

    private void OnDestroy()
    {
        TotalTrash--;
    }
}

public enum TrashType
{
    Organic,
    Paper,
    Plastic,
}
