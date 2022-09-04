namespace ControllerModule
{
    using UnityEngine;
    using ShipModule;

    public class ControllerPC : Controller
    {
        public GameObject MousePointPref;
        public LayerMask LayerForSelect;

        private GameObject MousePoint;
        private Ship _startShipPlayer;
        private Ship _startShipEnemy;
        private Camera _camera;

        public override void Position()
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);
                MousePoint.transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);

                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)/* || Input.GetTouch(0).phase == TouchPhase.Began*/)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(_camera.transform.position, ray.direction, out hit, Mathf.Infinity, LayerForSelect))
                    {
                        Debug.DrawRay(_camera.transform.position, ray.direction * hit.distance, Color.yellow);
                        var hitObject = hit.transform.gameObject;

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            if (hitObject.GetComponent<Ship>())
                            {
                                if(hitObject.GetComponent<Ship>().Role == Ship.ShipRole.Player)
                                {
                                    if(_startShipPlayer != null)
                                    {
                                        _startShipPlayer.TurnOnFrame(Ship.ShipRole.NoRole);
                                    }
                                    
                                    _startShipPlayer = hit.transform.gameObject.GetComponent<Ship>();
                                    _startShipPlayer.TurnOnFrame(_startShipPlayer.Role);
                                }
                                else if(hitObject.GetComponent<Ship>().Role == Ship.ShipRole.Enemy)
                                {
                                    if(_startShipEnemy != null)
                                    {
                                        _startShipEnemy.TurnOnFrame(Ship.ShipRole.NoRole);
                                    }

                                    _startShipEnemy = hit.transform.gameObject.GetComponent<Ship>();
                                    _startShipEnemy.TurnOnFrame(_startShipEnemy.Role);
                                }
                            }
                            else
                            {
                                if(_startShipPlayer != null)
                                {
                                    _startShipPlayer.TurnOnFrame(Ship.ShipRole.NoRole);
                                    _startShipPlayer = null;
                                }                                
                                
                                if(_startShipEnemy != null)
                                {
                                    _startShipEnemy.TurnOnFrame(Ship.ShipRole.NoRole);
                                    _startShipEnemy = null;
                                }
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.Mouse1))
                        {
                            if (_startShipPlayer != null)
                            {
                                if(hitObject.GetComponent<Ship>()?.Role == Ship.ShipRole.Enemy)
                                {
                                    if (_startShipEnemy != null)
                                    {
                                        _startShipEnemy.TurnOnFrame(Ship.ShipRole.NoRole);
                                    }

                                    _startShipEnemy = hitObject.GetComponent<Ship>();
                                    _startShipEnemy.TurnOnFrame(_startShipEnemy.Role);
                                    _startShipPlayer.Attack(_startShipEnemy);
                                }
                                else
                                {
                                    _startShipPlayer.Move(MousePoint.transform.position);
                                }
                            }
                        }

                    }
                }
            }
        }

        public override void Scroll()
        {

        }

        private void Start()
        {
            MousePoint = Instantiate(MousePointPref);
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            Position();
        }
    }

}