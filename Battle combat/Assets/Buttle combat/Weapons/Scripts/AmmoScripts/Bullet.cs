using ShipModule;
using UnityEngine;

public class Bullet : Ammunition
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        MoveAmmo();
        Destroy(gameObject, 10);
    }

    public override void HitTarget(GameObject target)
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 1f);

        target.GetComponent<Ship>().TakeDamage(AmmoInfo.Damage);

        Destroy(gameObject);
    }

    public override void MoveAmmo()
    {
        Vector3 rotateShoot = Target.position - transform.position;
        rb.AddForce(rotateShoot.normalized * AmmoInfo.Speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject?.GetComponent<Ship>() 
            && WeaponControllerAmmo.ShipScript.IsEnemy(WeaponControllerAmmo.ShipScript.Role, collision.gameObject.GetComponent<Ship>().Role))
        {
            HitTarget(collision.gameObject);
        }
    }
}
