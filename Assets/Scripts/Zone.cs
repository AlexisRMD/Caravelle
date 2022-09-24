using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public static Zone Instance;
    private void Awake()
    {
        Instance = this;
    }
    public List<Transform> DropPoints;

    public Vector3 GetDropPosition()
    {
        int index = 0;
        if (DropPoints.Count > 0)
        {
            index = (index + Random.Range(1, DropPoints.Count - 1)) % DropPoints.Count;
        }
        return DropPoints[index].position;
    }
}
