using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tableau : MonoBehaviour
{
    public static Tableau Instance;
    public List<Drop> historic = new();

    private void Awake()
    {
        Instance = this;
    }

    public void VerifyLink(GameObject g1, GameObject g2, LineController linkObj)
    {
        StoneData s1 = g1.GetComponent<Stone>().Data;
        StoneData s2 = g2.GetComponent<Stone>().Data;
        foreach (Drop link in historic)
        {
            if (link.hasBeenDrop) continue;
            bool thisLinkExist = false;
            foreach (Lien item in link.connectPierres)
            {
                if (item.Contains(s1, s2))
                {
                    thisLinkExist = true;
                    break;
                }
            }
            if (!thisLinkExist) continue;

            //this link is a good link, verify if other links are not also linked
            //if so, drop new stones, and remove if needed

            StartCoroutine(linkObj.ChangeColorValidate());



            break;
        }
    }
}

[System.Serializable]
public class Lien
{
    public StoneData pierre1;
    public StoneData pierre2;

    public bool Contains(StoneData s1, StoneData s2)
    {
        if ((pierre1.Equals(s1) && pierre2.Equals(s2)) || (pierre1.Equals(s2) && pierre2.Equals(s1))) return true;
        return false;
    }
}
[System.Serializable]
public class Drop
{
    public List<Lien> connectPierres = new();
    public List<StoneData> dropPierres = new();
    public bool hasBeenDrop = false;

}
