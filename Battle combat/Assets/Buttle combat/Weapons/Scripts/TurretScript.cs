using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    private Transform target;
    [HideInInspector] public WeaponController WeaponController;
    public List<GameObject> otherShips = new List<GameObject>();

    [Header("Attributes")]
    public float Range = 15f;
    public float TurnSpeed = 10f;
    public float FireRate = 1f;

    private float FireCountdown = 0f;

    [Header("Unit setup fields")]
    public string EnemyTag = "Enemy";
    public Transform PartToRotateHorizontal;
    public Transform PartToRotateVertical;

    public GameObject BulletPref;
    public Transform FirePoint;

    // Start is called before the first frame update
    void Start()
    {
        FindTargets();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void FindTargets()
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

    void UpdateTarget()
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

    // Update is called once per frame
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

    void Shoot()
    {
        GameObject bulletGO = Instantiate(BulletPref, FirePoint.position, FirePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

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
