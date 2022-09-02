using ShipModule;
using UnityEngine;

public class Missile : Ammunition
{
    private Transform _target;

    //public float Speed = 70f;
    //public float ExplosionRadius = 0f;
    public GameObject impactEffect;

    public void Seek(Transform target)
    {
        _target = target;
    }

    void Update()
    {
        if(_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = _target.position - transform.position;
        float distanceThisFrame = AmmoInfo.Speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(_target);
    }


    void Explode()
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

    void Damage(Transform enemy)
    {
        enemy.gameObject.GetComponent<Ship>().TakeDamage(AmmoInfo.Damage);
        //Destroy(enemy.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, AmmoInfo.ExplosionRadius);
    }

    public override void HitTarget()
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 1f);

        if (AmmoInfo.ExplosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(_target);
        }

        Destroy(gameObject);
    }
}
