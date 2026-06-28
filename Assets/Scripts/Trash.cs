using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Trash : MonoBehaviour
{
    [SerializeField] private TrashType _type;
    public TrashType Type => _type;

    private void Start()
    {
        ChooseType();
    }

    private void ChooseType()
    {
        _type = (TrashType)Random.Range(0, 3);

        Material mat = GetComponentInChildren<MeshRenderer>().material;

        if (mat == null) return;

        switch (_type)
        {
            case TrashType.Organic:
                mat.color = Color.darkOliveGreen;
                break;
            
            case TrashType.Paper:
                mat.color = Color.skyBlue;
                break;

            case TrashType.Plastic:
                mat.color = Color.yellowNice;
                break;
        }
    }
}

public enum TrashType
{
    Organic,
    Paper,
    Plastic,
}
