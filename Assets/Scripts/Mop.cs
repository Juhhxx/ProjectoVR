using UnityEngine;

public class Mop : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Vomit vomit = other.GetComponent<Vomit>();

        if (vomit != null)
        {
            Debug.Log("MOP TOUCHED VOMIT");
            vomit.Damage();
        }
    }
}
