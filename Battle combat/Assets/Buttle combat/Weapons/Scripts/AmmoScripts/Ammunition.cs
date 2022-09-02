using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    public AmmoData AmmoInfo;

    public abstract void HitTarget();
}
