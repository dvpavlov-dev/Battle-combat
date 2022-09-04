using UnityEngine;
using System;

public abstract class Ammunition : MonoBehaviour
{
    [Serializable]
    public struct AmmoData
    {
        public float Damage;
        public float Speed;
        public float ExplosionRadius;
    }

    [HideInInspector] public WeaponController WeaponControllerAmmo { get; private set; }

    public Transform Target;

    [SerializeField]
    public AmmoData AmmoInfo;

    public GameObject impactEffect;

    public abstract void HitTarget(GameObject target);
    public abstract void MoveAmmo();

    public void SetLinkWeaponController(WeaponController weaponControllerScript)
    {
        WeaponControllerAmmo = weaponControllerScript;
    }
}
