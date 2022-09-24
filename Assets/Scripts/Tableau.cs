using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tableau : MonoBehaviour
{
    public static Tableau Instance;
    public List<Drop> historic = new();

    public GameObject StoneCharacter;
    public GameObject StoneLocation;
    public GameObject StoneAction;
    public GameObject StoneConcept;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        DropItem(historic[0]);
    }

    private GameObject CreateStone(StoneData data)
    {
        GameObject newStone = null;
        switch (data.Type)
        {
            case StoneType.Character:
                newStone = Instantiate(StoneCharacter);
                break;

            case StoneType.Location:
                newStone = Instantiate(StoneLocation);
                break;

            case StoneType.Concept:
                newStone = Instantiate(StoneConcept);
                break;

            case StoneType.Action:
                newStone = Instantiate(StoneAction);
                break;

            default:
                break;
        }
        newStone.GetComponent<Stone>().Data = data;
        return newStone;
    }

    private void DropItem(Drop step)
    {
        foreach (StoneData item in step.dropPierres)
        {//create new stones
            CreateStone(item);
        }
        foreach (StoneData item in step.removePierres)
        {//remove stones
            SelectStone.Instance.RemoveStone(item);
        }
        step.hasBeenDrop = true;
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
            if (!thisLinkExist) continue; //TO DO : not good :( +1 error

            //this link is a good link, verify if other links are not also linked
            //if so, drop new stones, and remove if needed

            StartCoroutine(linkObj.ChangeColorValidate());

            bool allLinked = true;
            foreach(Lien item in link.connectPierres)
            {
                if (!SelectStone.Instance.LinkExist(item.pierre1, item.pierre2))
                { //there is not all links
                    allLinked = false;
                    break;
                }
            }
            if (!allLinked) break;

            DropItem(link);

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
    public int num;
    public List<Lien> connectPierres = new();
    public List<StoneData> dropPierres = new();
    public List<StoneData> removePierres = new();
    public bool hasBeenDrop = false;
    public bool isCheckpoint;
}
