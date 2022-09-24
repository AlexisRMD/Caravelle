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

    private void Move()
    {
        Vector3 newPosition = cam.transform.position;
        //Top
        if (Input.mousePosition.y >= Screen.height * 0.95 || Input.GetKey(KeyCode.Z) || (Input.GetMouseButton(2) && Input.GetAxis("Mouse Y") < 0))
        {
            newPosition += Vector3.forward * (Time.deltaTime * movingSensitivity);
        }
        //Down
        else if (Input.mousePosition.y <= Screen.height * 0.05 || Input.GetKey(KeyCode.S) || (Input.GetMouseButton(2) && Input.GetAxis("Mouse Y") > 0))
        {
            newPosition += -Vector3.forward * (Time.deltaTime * movingSensitivity);
        }
        // Right
        if (Input.mousePosition.x >= Screen.width * 0.95 || Input.GetKey(KeyCode.D) || (Input.GetMouseButton(2) && Input.GetAxis("Mouse X") < 0))
        {
            newPosition += cam.transform.right * (Time.deltaTime * movingSensitivity);
        }
        // Left
        else if (Input.mousePosition.x <= Screen.width * 0.05 || Input.GetKey(KeyCode.Q) || (Input.GetMouseButton(2) && Input.GetAxis("Mouse X") > 0))
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
        newPosition += Vector3.up * (Time.deltaTime * zoomSensitivity) * -Input.mouseScrollDelta.y;
        newPosition.y = Mathf.Clamp(newPosition.y, -Bounds.y, Bounds.y);
        cam.transform.position = newPosition;
    }
}
