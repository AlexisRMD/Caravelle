using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListLinks : MonoBehaviour
{
    public List<LineController> links = new();
    public List<LineController> Links() { return links; }
    public void AddLinks(LineController newLine) { links.Add(newLine); }
    public void RemoveLink(LineController newLine) { if(links.Contains(newLine)) links.Remove(newLine); }

    public IEnumerator ChangeColorValidate()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
    }
}
