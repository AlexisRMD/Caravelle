using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    [Header("Effect")]
    public Light AmbientLight;
    public float LightNormalIntensity = 1f;
    public float LightFadedIntensity = 0.5f;
    public float TimeToFadeLight = 0.5f;

    [Header("Dialogue")]
    public TMP_Text DialogueText;
    public float TextNormalAlpha = 1f;
    public float TextFadedAlpha = 0f;
    public float TimeToFadeText = 0.5f;

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
            gameObject.SetActive(true);
            DialogueIndex = 0;
            yield return StartCoroutine(FadeLight(LightNormalIntensity, LightFadedIntensity));
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
            yield return StartCoroutine(FadeLight(LightFadedIntensity, LightNormalIntensity));
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
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

    private IEnumerator FadeLight(float from, float to)
    {
        float timeElapsed = 0f;

        while (timeElapsed < TimeToFadeLight)
        {
            timeElapsed += Time.deltaTime;
            AmbientLight.intensity = Mathf.Lerp(from, to, timeElapsed / TimeToFadeLight);
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
