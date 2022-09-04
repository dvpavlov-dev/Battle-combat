using System.Collections.Generic;
using UnityEngine;
using ShipModule;

public class WeaponController : MonoBehaviour
{
    public GameObject PointsForTurrets;
    public List<GameObject> _turretsPref = new List<GameObject>();

    public Ship ShipScript { get; private set; }
    private List<GameObject> _turretPoints = new List<GameObject>();

    void Start()
    {
        if(PointsForTurrets != null)
        {
            AddPointsToList();
        }

        int i = 0;
        foreach(GameObject turret in _turretPoints)
        {
            var turret1 = Instantiate(_turretsPref[i], turret.transform);
            i++;
            turret1.GetComponent<Turret>().SetLinkWeaponController(this);
        }
    }

    public void SetLinkShip(Ship shipScript)
    {
        ShipScript = shipScript;
    }

    private void AddPointsToList()
    {
        foreach (Transform child in PointsForTurrets.transform)
        {
            _turretPoints.Add(child.gameObject);
        }
    }
}
