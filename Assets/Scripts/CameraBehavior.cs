using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBehavior : MonoBehaviour
{
    private Camera cam;
    public float movingSensitivity = 10f;
    public float zoomSensitivity = 10f;
    public Vector3 Bounds;


    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    private void Update()
    {
        Move();
        Zoom();
    }

    private Ray ray;
    private RaycastHit hitInfo;
    private Vector3 startPos;
    private Vector3 GetWorldPosition()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, 999f))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }
    private void Move()
    {
        if (Input.GetMouseButtonDown(2))
        {
            startPos = GetWorldPosition();
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 direction = startPos - GetWorldPosition();
            transform.position += direction;
        }
        
        Vector3 newPosition = cam.transform.position;
        //Top
        if (Input.mousePosition.y >= Screen.height * 0.95 || Input.GetKey(KeyCode.Z))
        {
            newPosition += Vector3.forward * (Time.deltaTime * movingSensitivity);
        }
        //Down
        else if (Input.mousePosition.y <= Screen.height * 0.05 || Input.GetKey(KeyCode.S))
        {
            newPosition += -Vector3.forward * (Time.deltaTime * movingSensitivity);
        }
        // Right
        if (Input.mousePosition.x >= Screen.width * 0.95 || Input.GetKey(KeyCode.D))
        {
            newPosition += cam.transform.right * (Time.deltaTime * movingSensitivity);
        }
        // Left
        else if (Input.mousePosition.x <= Screen.width * 0.05 || Input.GetKey(KeyCode.Q))
        {
            newPosition += -cam.transform.right * (Time.deltaTime * movingSensitivity);
        }
        newPosition.x = Mathf.Clamp(newPosition.x, -Bounds.x, Bounds.x);
        newPosition.z = Mathf.Clamp(newPosition.z, -Bounds.z, Bounds.z);
        cam.transform.position = newPosition;
    }

    private void Zoom()
    {
        Vector3 newPosition = cam.transform.position;
        newPosition += (Time.deltaTime * zoomSensitivity) * -Input.mouseScrollDelta.y * Vector3.up;
        newPosition.y = Mathf.Clamp(newPosition.y, 3.0f, Bounds.y);
        cam.transform.position = newPosition;
    }
}
