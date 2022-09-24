using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tableau : MonoBehaviour
{
    public static Tableau Instance;
    public List<Drop> historic = new();

    [Header("References Stones")]
    public GameObject StoneCharacter;
    public GameObject StoneLocation;
    public GameObject StoneAction;
    public GameObject StoneConcept;
    [Header("References Documents")]
    public GameObject Doc1;
    public GameObject Doc2;
    public GameObject Doc3;
    public GameObject Doc4;
    public GameObject Doc5;
    public GameObject Doc6;
    public GameObject Doc7;
    public GameObject Doc8;
    public GameObject Doc9;
    public GameObject Doc10;

    [HideInInspector] public int linksRemaining;
    private int errors = 0;
    private int actualStage = 1;

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
                newStone = Instantiate(StoneCharacter, Zone.Instance.GetDropPosition(),StoneCharacter.transform.rotation);
                break;

            case StoneType.Location:
                newStone = Instantiate(StoneLocation, Zone.Instance.GetDropPosition(), StoneLocation.transform.rotation);
                break;

            case StoneType.Concept:
                newStone = Instantiate(StoneConcept, Zone.Instance.GetDropPosition(), StoneConcept.transform.rotation);
                break;

            case StoneType.Action:
                newStone = Instantiate(StoneAction, Zone.Instance.GetDropPosition(), StoneAction.transform.rotation);
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

        //alternative connexions
        HashSet<int> sameStep = new HashSet<int>();
        sameStep.Add(15); sameStep.Add(16); sameStep.Add(17); sameStep.Add(18); sameStep.Add(19);
        if (sameStep.Contains(step.num))
        {
            foreach (var num in sameStep)
            {
                historic[num].hasBeenDrop = true;
            }
            actualStage = 19;
        }
    }

    public void VerifyLink(GameObject g1, GameObject g2, LineController linkObj)
    {
        StoneData s1 = g1.GetComponent<Stone>().Data;
        StoneData s2 = g2.GetComponent<Stone>().Data;
        bool thisLinkExist = false;
        foreach (Drop link in historic)
        {
            if (link.hasBeenDrop) continue;
            thisLinkExist = false;
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

            bool allLinked = true;
            linksRemaining = 0;
            foreach (Lien item in link.connectPierres)
            {
                if (!SelectStone.Instance.LinkExist(item.pierre1, item.pierre2))
                { //there is not all links
                    allLinked = false;
                    linksRemaining++;
                }
            }
            if (linksRemaining == 0 & link.num + 1 != historic.Count) linksRemaining = historic[link.num + 1].connectPierres.Count;
            SelectStone.Instance.SetTextLinksRemaining(linksRemaining);
            if (!allLinked) break;

            DropItem(link);
            if(historic[link.num + 1].isCheckpoint) errors = 0;
            actualStage++;

            break;
        }

        if (!thisLinkExist) {
            //not good :( +1 error + screenshake + remove link
            StartCoroutine(SelectStone.Instance.CameraShake());
            errors++;
            SelectStone.Instance.RemoveLink(g1, g2);
            linkObj.startObject.GetComponent<ListLinks>().RemoveLink(linkObj);
            linkObj.endObject.GetComponent<ListLinks>().RemoveLink(linkObj);
            Destroy(linkObj.gameObject);

            if (errors >= 3) {
                ReturnCheckpoint(actualStage);
                errors = 0;
            }
        }
    }

    private void ReturnCheckpoint(int returnStage)
    {
        if (returnStage < 0) return;

        if (!historic[returnStage].isCheckpoint)
        {
            HashSet<int> sameStep = new HashSet<int>();
            sameStep.Add(16); sameStep.Add(17); sameStep.Add(18); sameStep.Add(19);
            if (sameStep.Contains(returnStage))
            {
                actualStage = 15;
                return;
            }

            returnStage--;
            foreach (StoneData item in historic[returnStage].removePierres)
            {//create new stones
                CreateStone(item);
            }
            foreach (StoneData item in historic[returnStage].dropPierres)
            {//remove stones
                SelectStone.Instance.RemoveStone(item);
            }
            historic[returnStage].hasBeenDrop = false;
            ReturnCheckpoint(returnStage);
        }
        else
        {
            foreach (StoneData item in historic[returnStage - 1].dropPierres)
            {//remove links
                SelectStone.Instance.RemoveStone(item);
                CreateStone(item);
            }
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
