namespace ShipModule
{
    using UnityEngine;
    using System;
    using UnityEngine.AI;
    using ControllerModule;

    public abstract class Ship : MonoBehaviour
    {
        public GameObject ExplosionEffect;

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
            Neutral,
            NoRole
        }

        [Header("Роль корабля")]
        [SerializeField] public ShipRole Role;
        [SerializeField] public bool AI;
        [Header("Характеристики корабля")]
        [SerializeField] public ShipData ShipInfo;

        [HideInInspector] public GameObject PrimaryEnemy;

        private NavMeshAgent _agent;
        private WeaponController _weaponController;
        private UIController _uIController;

        private float _currentHealthHull;

        public virtual void Init()
        {
            _agent = gameObject.GetComponent<NavMeshAgent>();
            _weaponController = gameObject.GetComponent<WeaponController>();
            _weaponController.SetLinkShip(this);
            _uIController = gameObject.GetComponent<UIController>();

            _currentHealthHull = ShipInfo.HealthHull;

            if (AI)
            {
                CreateLinkControllerAI();
            }
        }

        public virtual void Move(Vector3 newPos)
        {
            _agent.SetDestination(newPos);
        }

        public virtual void Attack(Ship targetShip)
        {
            Debug.Log($"Attack {targetShip.gameObject} ship");
            PrimaryEnemy = targetShip.gameObject;
        }

        public virtual void Death()
        {
            var explosionEffect = Instantiate(ExplosionEffect, transform);
            Destroy(explosionEffect, 1);
            Destroy(this.gameObject, 1);
        }

        public bool IsEnemy(ShipRole myRole, ShipRole objectRole)
        {
            switch (myRole)
            {
                case ShipRole.Player:
                    if (objectRole == ShipRole.Enemy)
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

        public bool IsStillMoving()
        {
            return _agent.hasPath;
        }

        public void TurnOnFrame(ShipRole role)
        {
            _uIController.FrameScript.TurnOnFrame(role);
        }

        public void TakeDamage(float damage)
        {
            _currentHealthHull = _currentHealthHull - damage;
            _uIController.HealthScript.ReducingHealthStrip(ShipInfo.HealthHull, damage);

            if (_currentHealthHull < 0)
            {
                _uIController.HealthScript.ReducingHealthStrip(ShipInfo.HealthHull, damage);
                Death();
            }
        }

        //Need ControllerModule
        private void CreateLinkControllerAI()
        {
            ControllerAI bot = gameObject.AddComponent<ControllerAI>();
            bot.SetLinkShip(this);
        }
    }
}


