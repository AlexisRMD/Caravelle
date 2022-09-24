using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public Transform[] extremities = { null, null };

    private LineRenderer lr;
    private Vector3[] points = { Vector3.zero, Vector3.zero };

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        extremities[0] = gameObject.transform;
        points[0] = new Vector3(gameObject.transform.position.x, 0.4f, gameObject.transform.position.z);
        points[1] = gameObject.transform.position;
        lr.SetPositions(points);
    }
    private void Start()
    {
        /*
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;
        Mesh mesh = new Mesh();
        lr.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;
        */
    }

    public void FollowLine(Vector3 pos)
    {
        points[1] = new Vector3(pos.x, 0.4f, pos.z);
        lr.SetPositions(points);
    }

    public void UpdateLine()
    {
        for (int i = 0; i < extremities.Length; i++)
        {
            lr.SetPosition(i, extremities[i].position);
        }
    }
}
