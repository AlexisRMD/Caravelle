using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListLinks : MonoBehaviour
{
    public List<LineController> links = new();
    public List<LineController> Links() { return links; }
    public void AddLinks(LineController newLine) { links.Add(newLine); }
    public void RemoveLink(LineController newLine) { if(links.Contains(newLine)) links.Remove(newLine); }
    public void RemoveAllLinks()
    {
        foreach (LineController line in links)
        {
            SelectStone.Instance.RemoveLink(line.startObject, line.endObject);
            if (line.startObject != gameObject) line.startObject.GetComponent<ListLinks>().RemoveLink(line);
            else if (line.endObject != gameObject) line.endObject.GetComponent<ListLinks>().RemoveLink(line);
            Destroy(line.gameObject);
        }
        links.Clear();
    }
}
