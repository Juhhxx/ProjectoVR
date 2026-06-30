using UnityEngine;

public class Vomit : MonoBehaviour
{
    [SerializeField] private int _vomitHP = 3;
    private int _currentHP = 0;

    public static int TotalVomit = 0;

    private void Start()
    {
        _currentHP = _vomitHP;

        TotalVomit++;
    }

    public void Damage()
    {
        _currentHP--;

        float scale = (float)_currentHP/(float)_vomitHP;

        Debug.Log(scale);

        transform.localScale = Vector3.one * scale;

        if (_currentHP == 0) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        TotalVomit--;
    }
}
