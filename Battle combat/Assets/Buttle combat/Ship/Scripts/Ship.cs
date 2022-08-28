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


    //public float SpeedMove { get; set; }
    //public float SpeedRotate { get; set; }
    //public float HealthHull { get; set; }
    //public float HealthShield { get; set; }
    //public float ResistanceArmor { get; set; }

    private NavMeshAgent _agent;

    public virtual void Init()
    {
        _agent = gameObject.AddComponent<NavMeshAgent>();
    }

    public virtual void Move(Vector3 newPos)
    {
        _agent.SetDestination(newPos);
    }
}
