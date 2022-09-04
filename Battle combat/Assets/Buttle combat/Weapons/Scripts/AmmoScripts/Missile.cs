using ShipModule;
using UnityEngine;

public class Missile : Ammunition
{
    private Transform _target;

    private void Update()
    {
        if(Target == null)
        {
            Destroy(gameObject);
            return;
        }

        MoveAmmo();
    }

    public void Seek(Transform target)
    {
        Target = target;
    }

    public override void HitTarget(GameObject target)
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 1f);

        if (AmmoInfo.ExplosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target.transform);
        }

        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, AmmoInfo.ExplosionRadius);
        foreach(Collider hitObject in hitObjects)
        {
            if (hitObject.gameObject?.GetComponent<Ship>())
            {
                Damage(hitObject.transform);
            }
        }
    }

    private void Damage(Transform enemy)
    {
        enemy.gameObject.GetComponent<Ship>().TakeDamage(AmmoInfo.Damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, AmmoInfo.ExplosionRadius);
    }

    public override void MoveAmmo()
    {
        Vector3 dir = Target.position - transform.position;
        float distanceThisFrame = AmmoInfo.Speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget(Target.gameObject);
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(Target);
    }
}
