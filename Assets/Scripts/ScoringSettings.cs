using UnityEngine;

[CreateAssetMenu(fileName = "ScoringSettings", menuName = "Scriptable Objects/ScoringSettings")]
public class ScoringSettings : ScriptableObject
{
    [field: SerializeField] public float WrongTrashPenalization;
    [field: SerializeField] public float TrashPenalization;
    [field: SerializeField] public float VomitPenalization;
    [field: SerializeField] public float CleanTrashPoints;
    [field: SerializeField] public float CleanVomitPoints;
}
