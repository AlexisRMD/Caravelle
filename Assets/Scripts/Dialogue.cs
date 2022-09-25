using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [Header("Effect")]
    public Image FadeImage;
    public Color NormalColor;
    public Color FadedColor;
    public float TimeToFadeLight = 0.5f;

    [Header("Dialogue")]
    public TMP_Text DialogueText;
    public float TextNormalAlpha = 1f;
    public float TextFadedAlpha = 0f;
    public float TimeToFadeText = 0.5f;

    public DialogueData higherVolumeDialogue;

    public int DialogueIndex;

    public static Dialogue Instance;
    private void Awake()
    {
        Instance = this;
        DialogueIndex = 0;
        DialogueText.color = new Color(DialogueText.color.r, DialogueText.color.g, DialogueText.color.b, TextFadedAlpha);
        gameObject.SetActive(false);
    }

    public IEnumerator StartDialogue(DialogueData dd)
    {
        if (dd.Sentences.Count > 0)
        {
            if(dd == higherVolumeDialogue)
            {
                AudioPlay.Instance.audioSourceFx.volume = 0.75f;
            }
            else
            {
                AudioPlay.Instance.audioSourceFx.volume = 0.5f;
            }
            if(dd.dialogueSound != null) AudioPlay.Instance.PlayOneShot(dd.dialogueSound);
            gameObject.SetActive(true);
            CameraBehavior.Instance.CanMove = false;
            SelectStone.Instance.CanSelect = false;
            DialogueIndex = 0;
            yield return StartCoroutine(FadeLight(NormalColor, FadedColor));
            yield return StartCoroutine(ShowDialogue(dd));
            bool end = false;
            while(!end)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                {
                    // Next dialogue exist
                    if (dd.Sentences.Count - 1 > DialogueIndex)
                    {
                        yield return StartCoroutine(HideDialogue());
                        DialogueIndex++;
                        yield return StartCoroutine(ShowDialogue(dd));
                    }
                    else
                    {
                        yield return StartCoroutine(HideDialogue());
                        end = true;
                    }
                }
                
                yield return null;
            }
            yield return StartCoroutine(FadeLight(FadedColor, NormalColor));
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        CameraBehavior.Instance.CanMove = true;
        SelectStone.Instance.CanSelect = true;
        gameObject.SetActive(false);
    }

    private IEnumerator ShowDialogue(DialogueData dd)
    {
        DialogueText.text = dd.Sentences[DialogueIndex];
        yield return StartCoroutine(FadeDialogue(TextFadedAlpha, TextNormalAlpha));
    }

    private IEnumerator HideDialogue()
    {
        yield return StartCoroutine(FadeDialogue(TextNormalAlpha, TextFadedAlpha));
    }

    private IEnumerator FadeLight(Color from, Color to)
    {
        float timeElapsed = 0f;

        while (timeElapsed < TimeToFadeLight)
        {
            timeElapsed += Time.deltaTime;
            Color newColor = Color.Lerp(from, to, timeElapsed / TimeToFadeLight);
            FadeImage.color = newColor;
            yield return null;
        }
    }

    private IEnumerator FadeDialogue(float from, float to)
    {
        float timeElapsed = 0f;

        while (timeElapsed < TimeToFadeLight)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, timeElapsed / TimeToFadeText);
            DialogueText.color = new Color(DialogueText.color.r, DialogueText.color.g, DialogueText.color.b, alpha);
            yield return null;
        }
    }

}
