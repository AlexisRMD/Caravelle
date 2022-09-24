using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public GameObject startObject = null;
    public GameObject endObject = null;



    public void FollowLine(Vector3 pos)
    {
        Vector3 startPos = startObject.transform.position;
        startPos.y = 0.25f;
        pos.y = 0.25f;

        float distance = Vector3.Distance(startPos, pos);
        transform.localScale = new Vector3(transform.localScale.x, distance / 2, transform.localScale.z);

        Vector3 middlePoint = (startPos + pos) / 2f;
        transform.position = middlePoint;

        transform.up = (pos - startPos); //rotation
    }

    public void UpdateLine()
    {
        Vector3 startPos = startObject.transform.position;
        startPos.y = 0.25f;
        Vector3 endPos = endObject.transform.position;
        endPos.y = 0.25f;

        float distance = Vector3.Distance(startPos, endPos);
        transform.localScale = new Vector3(transform.localScale.x, distance / 2, transform.localScale.z);

        Vector3 middlePoint = (startPos + endPos) / 2f;
        transform.position = middlePoint;

        transform.up = (endPos - startPos); //rotation

    }


}
