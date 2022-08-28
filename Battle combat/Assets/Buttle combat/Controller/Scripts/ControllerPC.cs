using UnityEngine;

public class ControllerPC : PlayerController
{
    public GameObject TestCube;

    private Ship _startShip;
    private Camera _camera;

    public override void Position()
    {
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position);
            TestCube.transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);

            if (Input.GetKeyDown(KeyCode.Mouse0)/* || Input.GetTouch(0).phase == TouchPhase.Began*/)
            {
                RaycastHit hit;
                if (Physics.Raycast(_camera.transform.position, ray.direction, out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(_camera.transform.position, ray.direction * hit.distance, Color.yellow);
                    if (hit.transform.gameObject.tag == "PlayerShip")
                    {
                        _startShip = hit.transform.gameObject.GetComponent<Ship>();
                    }
                    else
                    {
                        if(_startShip != null)
                        {
                            _startShip.Move(TestCube.transform.position);
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
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        Position();
    }
}
