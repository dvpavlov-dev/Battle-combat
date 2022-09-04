using UnityEngine;

public class TurretMachineGun : Turret
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
}
