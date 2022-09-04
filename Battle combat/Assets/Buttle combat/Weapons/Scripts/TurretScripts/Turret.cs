using System.Collections.Generic;
using UnityEngine;
using System;
using ShipModule;

public abstract class Turret : MonoBehaviour
{
    [Serializable]
    public struct TurretData
    {
        public float Range;
        public float TurnSpeed;
        public float FireRate;
    }

    [Header("Attributes")]
    public TurretData TurretInfo;

    public GameObject BulletPref;
    public GameObject MuzzleEffect;
    public Transform FirePoint;

    [Header("Unit setup fields")]
    public Transform PartToRotateHorizontal;
    public Transform PartToRotateVertical;

    [HideInInspector] public WeaponController WeaponControllerTurret { get; private set; }
    public List<GameObject> OtherShips = new List<GameObject>();

    protected Transform target;
    protected GameObject bullet;
    private float FireCountdown = 0f;

    void Update()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(PartToRotateHorizontal.rotation, lookRotation, Time.deltaTime * TurretInfo.TurnSpeed).eulerAngles;
        PartToRotateHorizontal.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);

        if (FireCountdown <= 0f)
        {
            Shoot();
            FireCountdown = 1f / TurretInfo.FireRate;
        }
        FireCountdown -= Time.deltaTime;
    }

    public virtual void Init()
    {
        GetComponent<SphereCollider>().radius = TurretInfo.Range;
        FindTargets();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    public void SetLinkWeaponController(WeaponController weaponControllerScript)
    {
        WeaponControllerTurret = weaponControllerScript;
    }

    public virtual void Shoot()
    {
        GameObject muzzleEffect = Instantiate(MuzzleEffect, FirePoint.position, FirePoint.rotation);
        Destroy(muzzleEffect, 0.5f);
        bullet = Instantiate(BulletPref, FirePoint.position, FirePoint.rotation);
        var ammoScript = bullet.GetComponent<Ammunition>();
        ammoScript.Target = target;
        ammoScript.SetLinkWeaponController(WeaponControllerTurret);
    }

    private void FindTargets()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, TurretInfo.Range, LayerMask.GetMask("Selected object"));

        foreach (Collider obj in hitColliders)
        {
            if (obj.gameObject?.GetComponent<Ship>()
                && WeaponControllerTurret.ShipScript.IsEnemy(WeaponControllerTurret.ShipScript.Role, obj.gameObject.GetComponent<Ship>().Role))
            {
                //Debug.Log($"Ship {WeaponControllerTurret.ShipScript.gameObject}: {obj.gameObject}");
                OtherShips.Add(obj.gameObject);
            }
        }
    }

    private void UpdateTarget()
    {
        OtherShips.RemoveAll(item => item == null);
        if (OtherShips.Count == 0)
        {
            target = null;
            return;
        }

        if (WeaponControllerTurret.ShipScript.PrimaryEnemy != null)
        {
            CheckDistance(WeaponControllerTurret.ShipScript.PrimaryEnemy);
        }

        if(target == null)
        {
            CheckDistance(OtherShips);
        }
    }

    private void CheckDistance(List<GameObject> otherShips)
    {
        float shortestdistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in otherShips)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestdistance)
            {
                shortestdistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

            if (nearestEnemy != null && shortestdistance <= TurretInfo.Range)
            {
                target = nearestEnemy.transform;
            }
            else
            {
                target = null;
            }
        }
    }

    private void CheckDistance(GameObject enemy)
    {
        float shortestdistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
        if (distanceToEnemy < shortestdistance)
        {
            shortestdistance = distanceToEnemy;
            nearestEnemy = enemy;
        }

        if (nearestEnemy != null && shortestdistance <= TurretInfo.Range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject?.GetComponent<Ship>()
            && WeaponControllerTurret.ShipScript.IsEnemy(WeaponControllerTurret.ShipScript.Role, other.gameObject.GetComponent<Ship>().Role))
        {
            OtherShips.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject?.GetComponent<Ship>()
            && WeaponControllerTurret.ShipScript.IsEnemy(WeaponControllerTurret.ShipScript.Role, other.gameObject.GetComponent<Ship>().Role))
        {
            OtherShips.Remove(other.gameObject);
        }
    }
}
