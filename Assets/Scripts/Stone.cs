using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum StoneType
{
    Character,
    Location,
    Concept,
    Action
}
public abstract class Stone<T> : MonoBehaviour where T: StoneData
{
    public StoneType Type;
    public Image Image;
    public TMP_Text Name;
    public TMP_Text Description;
    public T Data;

    public float HoverTime = 0.5f;

    private void Start()
    {
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
    }

    private void OnMouseOver()
    {
        timer += Time.deltaTime;
        if(timer > HoverTime)
        {
            Description.gameObject.SetActive(true);
        }
    }
    private void OnMouseExit()
    {
        Description.gameObject.SetActive(false);
        timer = 0f;
    }
}
