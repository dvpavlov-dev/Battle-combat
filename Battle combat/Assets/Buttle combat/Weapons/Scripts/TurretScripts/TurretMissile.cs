using UnityEngine;

public class TurretMissile : Turret
{
    void Start()
    {
        Init();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, TurretInfo.Range);
    }

    public override void Shoot()
    {
        base.Shoot();
        Missile rocket = bullet.GetComponent<Missile>();

        if (rocket != null)
        {
            rocket.Seek(target);
        }
    }
}
