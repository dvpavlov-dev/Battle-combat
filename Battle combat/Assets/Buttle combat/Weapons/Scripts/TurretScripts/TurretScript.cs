using System.Collections.Generic;
using UnityEngine;
using ShipModule;

public class TurretScript : MonoBehaviour
{
    private Transform target;
    [HideInInspector] public WeaponController WeaponController { get; private set; }
    public List<GameObject> otherShips = new List<GameObject>();

    [Header("Attributes")]
    public float Range = 15f;
    public float TurnSpeed = 10f;
    public float FireRate = 1f;

    private float FireCountdown = 0f;

    [Header("Unit setup fields")]
    public Transform PartToRotateHorizontal;
    public Transform PartToRotateVertical;

    public GameObject BulletPref;
    public Transform FirePoint;

    void Start()
    {
        FindTargets();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void Update()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(PartToRotateHorizontal.rotation, lookRotation, Time.deltaTime * TurnSpeed).eulerAngles;
        PartToRotateHorizontal.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);

        if (FireCountdown <= 0f)
        {
            Shoot();
            FireCountdown = 1f / FireRate;
        }
        FireCountdown -= Time.deltaTime;

    }

    public void SetLinkWeaponController(WeaponController weaponControllerScript)
    {
        WeaponController = weaponControllerScript;
    }

    private void FindTargets()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Range, LayerMask.GetMask("Selected object"));

        foreach (Collider obj in hitColliders)
        {
            if (obj.gameObject?.GetComponent<Ship>()
                && WeaponController.ShipScript.IsEnemy(WeaponController.ShipScript.Role, obj.gameObject.GetComponent<Ship>().Role))
            {
                otherShips.Add(obj.gameObject);
            }
        }
    }

    private void UpdateTarget()
    {
        if (otherShips.Count == 0)
        {
            target = null;
            return;
        }

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

            if (nearestEnemy != null && shortestdistance <= Range)
            {
                target = nearestEnemy.transform;
            }
            else
            {
                target = null;
            }
        }

    }

    private void Shoot()
    {
        GameObject bulletGO = Instantiate(BulletPref, FirePoint.position, FirePoint.rotation);
        Missile bullet = bulletGO.GetComponent<Missile>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject?.GetComponent<Ship>()
            && WeaponController.ShipScript.IsEnemy(WeaponController.ShipScript.Role, other.gameObject.GetComponent<Ship>().Role))
        {
            otherShips.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject?.GetComponent<Ship>()
            && WeaponController.ShipScript.IsEnemy(WeaponController.ShipScript.Role, other.gameObject.GetComponent<Ship>().Role))
        {
            otherShips.Remove(other.gameObject);
        }
    }
}
