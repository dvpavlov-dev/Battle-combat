namespace ControllerModule
{
    using System.Collections.Generic;
    using UnityEngine;
    using ShipModule;

    public class ControllerAI : Controller
    {
        public Ship ShipScript { get; private set; }

        private List<Vector3> _routePoints = new List<Vector3>();
        private float rangeArea = 25f;
        private int currentPount = 0;

        public override void Position()
        {
            currentPount++;
            if (currentPount == _routePoints.Count)
            {
                currentPount = 0;
            }

            ShipScript.Move(_routePoints[currentPount]);
        }

        public override void Scroll()
        {

        }

        // This is test way for now
        private void CreateWay()
        {
            Vector3 point1 = new Vector3(transform.position.x - rangeArea, transform.position.y, transform.position.z + rangeArea);
            _routePoints.Add(point1);
            Vector3 point2 = new Vector3(transform.position.x + rangeArea, transform.position.y, transform.position.z + rangeArea);
            _routePoints.Add(point2);
            Vector3 point3 = new Vector3(transform.position.x + rangeArea, transform.position.y, transform.position.z - rangeArea);
            _routePoints.Add(point3);
            Vector3 point4 = new Vector3(transform.position.x - rangeArea, transform.position.y, transform.position.z - rangeArea);
            _routePoints.Add(point4);

            Position();
        }

        void Start()
        {
            CreateWay();
        }

        void Update()
        {
            if (!ShipScript.IsStillMoving())
            {
                Position();
            }
        }

        public void SetLinkShip(Ship shipScript)
        {
            ShipScript = shipScript;
        }
    }
}

