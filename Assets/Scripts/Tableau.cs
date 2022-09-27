using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tableau : MonoBehaviour
{
    public static Tableau Instance;
    public List<Drop> historic = new();

    [Header("References Stones")]
    public GameObject StoneCharacter;
    public GameObject StoneLocation;
    public GameObject StoneAction;
    public GameObject StoneConcept;
    public GameObject StoneConseil;
    public GameObject StoneSmallConcept;
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
    public GameObject DocControles;
    public GameObject DocMap;
    private Dictionary<int, GameObject> docs = new();
    [Header("References Dialogue")]
    public DialogueData Introduction;
    public DialogueData ThreeOne;
    public DialogueData FourOne;
    public DialogueData SixOne;
    public DialogueData Epilogue;
    private Dictionary<int, DialogueData> dialogues = new();

    public Button ReturnToMainMenuBtn;
    
    [HideInInspector] public int linksRemaining;
    private int errors = 0;
    private int actualStage = 1;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ReturnToMainMenuBtn.gameObject.SetActive(false);
        docs.Add(1, Doc1);
        docs.Add(2, Doc2);
        docs.Add(3, Doc3);
        docs.Add(4, Doc4);
        docs.Add(5, Doc5);
        docs.Add(6, Doc6);
        docs.Add(7, Doc7);
        docs.Add(8, Doc8);
        docs.Add(9, Doc9);
        docs.Add(10, Doc10);
        docs.Add(11, DocControles);
        docs.Add(12, DocMap);
        dialogues.Add(1, ThreeOne);
        dialogues.Add(2, FourOne);
        dialogues.Add(3, SixOne);
        dialogues.Add(4, Epilogue);
        DropItem(historic[0]);
        Instantiate(docs[11]);
        Instantiate(docs[12]);
        StartCoroutine(Dialogue.Instance.StartDialogue(Introduction));
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
            case StoneType.Conseil:
                newStone = Instantiate(StoneConseil, Zone.Instance.GetDropPosition(), StoneConseil.transform.rotation);
                break;
            case StoneType.SmallConcept:
                newStone = Instantiate(StoneSmallConcept, Zone.Instance.GetDropPosition(), StoneSmallConcept.transform.rotation);
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

        if (step.doc > -1 && !step.docIsPlaced)
        {
            Instantiate(docs[step.doc]);
            step.docIsPlaced = true;
        }

        if (step.dialogue > -1 && !step.dialogueHasBeenSaid)
        {
            StartCoroutine(Dialogue.Instance.StartDialogue(dialogues[step.dialogue]));
            step.dialogueHasBeenSaid = true;
            if (step.num == 14) AudioPlay.Instance.PlayMusic(AudioPlay.Instance.music2);
            if (step.num == 28) ReturnToMainMenuBtn.gameObject.SetActive(true);
        }
        AudioPlay.Instance.PlayOneShot(AudioPlay.Instance.newStone);

        //alternative connexions
        HashSet<int> sameStep = new HashSet<int>();
        sameStep.Add(16); sameStep.Add(17);
        if (sameStep.Contains(step.num))
        {
            foreach (var num in sameStep)
            {
                historic[num].hasBeenDrop = true;
            }
            actualStage = 17;
        }
        sameStep.Clear();
        sameStep.Add(8); sameStep.Add(9);
        if (sameStep.Contains(step.num))
        {
            foreach (var num in sameStep)
            {
                historic[num].hasBeenDrop = true;
            }
            actualStage = 9;
        }
        sameStep.Clear();
        sameStep.Add(20); sameStep.Add(21);
        if (sameStep.Contains(step.num))
        {
            foreach (var num in sameStep)
            {
                historic[num].hasBeenDrop = true;
            }
            actualStage = 20;
        }
        sameStep.Clear();
        sameStep.Add(23); sameStep.Add(24); sameStep.Add(25);
        if (sameStep.Contains(step.num))
        {
            foreach (var num in sameStep)
            {
                historic[num].hasBeenDrop = true;
            }
            actualStage = 23;
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


            AudioPlay.Instance.PlayOneShot(AudioPlay.Instance.yes);
            //this link is a good link, verify if other links are not also linked
            //if so, drop new stones, and remove if needed

            Coroutine yellow = StartCoroutine(linkObj.ChangeColorValidate(Color.yellow));
            linkObj.ColorCoroutine = yellow;

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
            if(yellow != null)
            {
                StopCoroutine(yellow);
            }
            LineController[] foundStones = FindObjectsOfType<LineController>();
            foreach (LineController st in foundStones)
            {
                st.gameObject.GetComponent<MeshRenderer>().material.color = Color.grey;
            }

            if (historic[link.num-1].isCheckpoint)
            {
                errors = 0;
                AudioPlay.Instance.PlayOneShot(AudioPlay.Instance.checkpoint);
            }
            actualStage++;

            break;
        }

        if (!thisLinkExist) {
            //not good :( +1 error + screenshake + remove link

            AudioPlay.Instance.PlayOneShot(AudioPlay.Instance.no);
            StartCoroutine(SelectStone.Instance.CameraShake());
            errors++;
            SelectStone.Instance.RemoveLink(g1, g2);
            linkObj.startObject.GetComponent<ListLinks>().RemoveLink(linkObj);
            linkObj.endObject.GetComponent<ListLinks>().RemoveLink(linkObj);
            Destroy(linkObj.gameObject);

            if (errors >= 3) {
                ReturnCheckpoint(actualStage);
                AudioPlay.Instance.PlayOneShot(AudioPlay.Instance.swipBoard);
                errors = 0;
            }
        }
    }

    private void ReturnCheckpoint(int returnStage)
    {

        if (returnStage < 0) return;

        if (!historic[returnStage].isCheckpoint)
        {
            if (returnStage==17)
            {
                actualStage = 16;
                return;
            }
            if (returnStage == 9)
            {
                actualStage = 8;
                return;
            }
            if (returnStage == 21)
            {
                actualStage = 20;
                return;
            }
            if (returnStage == 24 || returnStage == 25)
            {
                actualStage = 23;
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
            if (returnStage == 0) returnStage++;
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
    public int hasAlternativeLinkNumber = -1;

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
    [HideInInspector] public bool hasBeenDrop = false;
    public bool isCheckpoint;
    public int doc = -1;
    public int dialogue = -1;
    [HideInInspector] public bool docIsPlaced = false;
    [HideInInspector] public bool dialogueHasBeenSaid = false;
}
