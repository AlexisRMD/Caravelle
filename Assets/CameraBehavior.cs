using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBehavior : MonoBehaviour
{
    private Camera cam;
    public float movingSensitivity = 10f;
    public float zoomSensitivity = 10f;

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
        //Top
        if (Input.mousePosition.y >= Screen.height * 0.95 || Input.GetKey(KeyCode.Z) || (Input.GetMouseButton(2) && Input.GetAxis("Mouse Y") < 0))
        {
            cam.transform.position += Vector3.forward * (Time.deltaTime * movingSensitivity);
        }
        //Down
        else if (Input.mousePosition.y <= Screen.height * 0.05 || Input.GetKey(KeyCode.S) || (Input.GetMouseButton(2) && Input.GetAxis("Mouse Y") > 0))
        {
            cam.transform.position += -Vector3.forward * (Time.deltaTime * movingSensitivity);
        }
        // Right
        if (Input.mousePosition.x >= Screen.width * 0.95 || Input.GetKey(KeyCode.D) || (Input.GetMouseButton(2) && Input.GetAxis("Mouse X") < 0))
        {
            cam.transform.position += cam.transform.right * (Time.deltaTime * movingSensitivity);
        }
        // Left
        else if (Input.mousePosition.x <= Screen.width * 0.05 || Input.GetKey(KeyCode.Q) || (Input.GetMouseButton(2) && Input.GetAxis("Mouse X") > 0))
        {
            cam.transform.position += -cam.transform.right * (Time.deltaTime * movingSensitivity);
        }
    }

    private void Zoom()
    {

        cam.transform.position += Vector3.up * (Time.deltaTime * zoomSensitivity) * -Input.mouseScrollDelta.y;
    }
}
