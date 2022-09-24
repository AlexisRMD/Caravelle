using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public enum StoneType
{
    Character,
    Location,
    Concept,
    Action
}
public class Stone : MonoBehaviour
{
    public StoneType Type;
    public Image Image;
    public TMP_Text Name;
    public TMP_Text Description;
    public StoneData Data;

    [Header("Refs")]
    public Outline Outline;
    public Canvas canvas;

    public float HoverTime = 0.5f;
    public ListLinks links;
    [HideInInspector] public Quaternion rotationInit;

    private void Start()
    {
        rotationInit = gameObject.transform.rotation;

        canvas.sortingOrder = 0;
        Outline.enabled = false;
        Description.gameObject.SetActive(false);
        if (Data != null)
        {
            Image.sprite = Data.Sprite;
            if(Image.sprite != null)
            {
                Image.preserveAspect = true;
            }
            Name.text = Data.name;
            Description.text = Data.Description;
        }
    }


    private float timer = 0f;
    private void OnMouseDrag()
    {
        timer = 0f;

        foreach (LineController link in links.Links())
        {
            link.UpdateLine();
        }
    }


    private void OnMouseOver()
    {
        Outline.enabled = true;
        canvas.sortingOrder = 9;
        timer += Time.deltaTime;
        if(timer > HoverTime)
        {
            Description.gameObject.SetActive(true);
        }
    }
    private void OnMouseExit()
    {
        Outline.enabled = false;
        Description.gameObject.SetActive(false);
        canvas.sortingOrder = 0;
        timer = 0f;
    }

}
