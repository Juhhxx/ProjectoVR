using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class ClientEatingBehaviour : MonoBehaviour
{
    [SerializeField, MinMaxSlider(2, 15)] private Vector2 _minMaxEatingTime;

    public IEnumerator EatingBehaviour()
    {
        Debug.Log($"EATING {name}");

        yield return new WaitForSeconds(Random.Range(_minMaxEatingTime.x, _minMaxEatingTime.y));

        yield return null;
    }
}
