using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListLinks : MonoBehaviour
{
    public List<LineController> links = new();
    public List<LineController> Links() { return links; }
    public void AddLinks(LineController newLine) { links.Add(newLine); }
}
