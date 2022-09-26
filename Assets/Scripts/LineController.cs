using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public GameObject startObject = null;
    public GameObject endObject = null;

    [HideInInspector] public float posY = -.2f;

    public void FollowLine(Vector3 pos)
    {
        Vector3 startPos = startObject.transform.position;
        startPos.y = posY;
        pos.y = posY;

        float distance = Vector3.Distance(startPos, pos);
        transform.localScale = new Vector3(transform.localScale.x, distance / 2, transform.localScale.z);

        Vector3 middlePoint = (startPos + pos) / 2f;
        transform.position = middlePoint;

        transform.up = (pos - startPos); //rotation
    }

    public void UpdateLine()
    {
        Vector3 startPos = startObject.transform.position;
        startPos.y = posY;
        Vector3 endPos = endObject.transform.position;
        endPos.y = posY;

        float distance = Vector3.Distance(startPos, endPos);
        transform.localScale = new Vector3(transform.localScale.x, distance / 2, transform.localScale.z);

        Vector3 middlePoint = (startPos + endPos) / 2f;
        transform.position = middlePoint;

        transform.up = (endPos - startPos); //rotation

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && gameObject.GetComponent<MeshRenderer>().material.color != Color.grey)
        {
            SelectStone.Instance.RemoveLink(startObject, endObject);
            startObject.GetComponent<ListLinks>().RemoveLink(this);
            endObject.GetComponent<ListLinks>().RemoveLink(this);
            if(ColorCoroutine != null)
            {
                StopCoroutine(ColorCoroutine);
                ColorCoroutine = null;
            }
            Destroy(gameObject);
        }
    }

    public Coroutine ColorCoroutine;
    public IEnumerator ChangeColorValidate(Color newColor)
    {
        yield return new WaitForSecondsRealtime(0.4f);
        
        Color initialColor = gameObject.GetComponent<MeshRenderer>().material.color;
        for (float i = 0; i < 1; i+=0.2f)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(initialColor, newColor, i);
            yield return new WaitForFixedUpdate();
        }
    }

}
