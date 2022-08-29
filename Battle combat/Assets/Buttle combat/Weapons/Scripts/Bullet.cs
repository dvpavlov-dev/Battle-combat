using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _target;

    public float Speed = 70f;
    public float ExplosionRadius = 0f;
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
        float distanceThisFrame = Speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(_target);
    }

    private void HitTarget()
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        if(ExplosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(_target);
        }

        Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach(Collider hitObject in hitObjects)
        {
            Damage(hitObject.transform);
        }
    }

    void Damage(Transform enemy)
    {
        //Destroy(enemy.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, ExplosionRadius);
    }
}
