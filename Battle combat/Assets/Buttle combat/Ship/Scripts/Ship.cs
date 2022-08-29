using UnityEngine;
using System;
using UnityEngine.AI;

public abstract class Ship : MonoBehaviour
{
    [Serializable]
    public struct ShipData
    {
        public float SpeedMove;
        public float SpeedRotate;
        public float HealthHull;
        public float HealthShield;
        public float ResistanceArmor;

        public ShipData(float speedMove, float speedRotate, float healthHull, float healthShield, float resistanceArmor)
        {
            SpeedMove = speedMove;
            SpeedRotate = speedRotate;
            HealthHull = healthHull;
            HealthShield = healthShield;
            ResistanceArmor = resistanceArmor;
        }
    }

    [Serializable]
    public enum ShipRole
    {
        Player,
        Enemy,
        Neutral
    }

    [Header("Роль корабля")]
    [SerializeField] public ShipRole Role;
    [Header("Характеристики корабля")]
    [SerializeField] public ShipData shipData;

    //public ShipRole MyRole { get; private set; }
    //public ShipRole EnemyRole { get; private set; }

    private NavMeshAgent _agent;
    private WeaponController _weaponController;

    public virtual void Init()
    {
        _agent = gameObject.AddComponent<NavMeshAgent>();
        _weaponController = gameObject.GetComponent<WeaponController>();
        _weaponController.ShipScript = this;
        //ChooseRole(Role);
    }

    public virtual void Move(Vector3 newPos)
    {
        _agent.SetDestination(newPos);
    }

    public bool IsEnemy(ShipRole myRole, ShipRole objectRole)
    {
        switch (myRole)
        {
            case ShipRole.Player:
                if(objectRole == ShipRole.Enemy)
                {
                    return true;    
                }
                break;
            case ShipRole.Enemy:
                if (objectRole == ShipRole.Player)
                {
                    return true;
                }
                break;
        }

        return false;
    }

    //private void ChooseRole(ShipRole shipRole)
    //{
    //    switch (shipRole)
    //    {
    //        case ShipRole.Player:
    //            MyRole = ShipRole.Player;
    //            EnemyRole = ShipRole.Enemy;
    //            break;
    //        case ShipRole.Enemy:
    //            MyRole = ShipRole.Enemy;
    //            EnemyRole = ShipRole.Player;
    //            break;
    //    }
    //}
}
