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
        index = -1;
    }
    public List<Transform> DropPoints;

    public Vector3 GetDropPosition()
    {
        if (index < DropPoints.Count-1)
        {
            index++;
        }
        else
        {
            index = 0;
        }
        return DropPoints[index].position;
    }
}
