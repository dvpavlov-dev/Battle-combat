using UnityEngine;

public class StartShip : Ship
{
    [Header("�������������� �������")]
    [SerializeField] public ShipData shipData;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
}
