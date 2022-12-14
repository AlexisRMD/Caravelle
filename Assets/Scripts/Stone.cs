using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public enum StoneType
{
    Character,
    Location,
    Concept,
    Action,
    Conseil,
    SmallConcept
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
    public Canvas DescriptionCanvas;

    public float HoverTime = 0.5f;
    public ListLinks links;
    [HideInInspector] public Quaternion rotationInit;

    private void Start()
    {
        rotationInit = gameObject.transform.rotation;

        DescriptionCanvas.sortingOrder = 0;
        Outline.enabled = false;
        DescriptionCanvas.gameObject.SetActive(false);
        if (Data != null)
        {
            Image.sprite = Data.Sprite;
            if(Image.sprite != null)
            {
                Image.preserveAspect = true;
            }
            Name.text = Data.Name;
            Description.text = Data.Description;
        }
    }

    public IEnumerator RemoveIt()
    {
        Vector3 initScale = gameObject.transform.localScale;
        for (float i = 0; i < 1; i+=0.1f)
        {
            gameObject.transform.localScale -= Vector3.one * 0.1f ;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        if (transform.position.y < -3f)
        {
            transform.position = Zone.Instance.GetDropPosition();
        }

        foreach (LineController link in links.Links())
        {
            link.UpdateLine();
        }
    }


    private float timer = 0f;
    private void OnMouseDrag()
    {
        timer = 0f;
    }


    private void OnMouseOver()
    {
        Debug.Log("on " + Name);
        Outline.enabled = true;
        DescriptionCanvas.sortingOrder = 9;
        timer += Time.deltaTime;
        if(timer > HoverTime && Data.Description != "")
        {
            DescriptionCanvas.gameObject.SetActive(true);
        }
    }
    private void OnMouseExit()
    {
        Outline.enabled = false;
        DescriptionCanvas.gameObject.SetActive(false);
        DescriptionCanvas.sortingOrder = 0;
        timer = 0f;
    }

}
