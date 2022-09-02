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

        foreach(GameObject turret in _turretPoints)
        {
            var turret1 = Instantiate(_turretsPref[1], turret.transform);
            turret1.GetComponent<TurretScript>().SetLinkWeaponController(this);
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
