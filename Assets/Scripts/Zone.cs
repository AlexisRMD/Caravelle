using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public static Zone Instance;
    int index = 0;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        index = 0;
    }
    public List<Transform> DropPoints;

    public Vector3 GetDropPosition()
    {
        if (DropPoints.Count > 0)
        {
            index = (index + Random.Range(1, DropPoints.Count - 1)) % DropPoints.Count;
        }
        return DropPoints[index].position;
    }
}
