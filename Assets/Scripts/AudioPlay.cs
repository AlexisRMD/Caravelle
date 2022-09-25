using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioPlay : MonoBehaviour
{
    public AudioSource audioSourceFx;
    public AudioSource audioSourceMusique;

    [Header("Sound References")]
    public AudioClip music1;
    public AudioClip music2;
    public AudioClip swipBoard;
    public AudioClip checkpoint;
    public AudioClip no;
    public AudioClip yes;
    public AudioClip upStone;
    public AudioClip placeStone;
    public AudioClip newStone;
    public AudioClip upDoc;
    public AudioClip placeDoc;
    public AudioClip button;
    public AudioClip traceLink;

    public void EnableMusic(bool play = true)
    {
        audioSourceMusique.mute = !play;
    }
    public void EnableSfx(bool play = true)
    {
        audioSourceFx.mute = !play;
    }

    public void PlayOneShot(AudioClip clip)
    {
        audioSourceFx.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip music)
    {
        StartCoroutine(PlayMusicAsync(music));
    }

    public void TraceLine()
    {
        audioSourceFx.clip = traceLink;
        audioSourceFx.Play();
    }
    public void StopLine()
    {
        if(audioSourceFx.clip == traceLink)
            audioSourceFx.Stop();
    }

    private IEnumerator PlayMusicAsync(AudioClip music)
    {
        if (audioSourceMusique.isPlaying)
        {
            float volumeMusic = audioSourceMusique.volume;
            float timeFade = 1f;
            float step = volumeMusic / (timeFade * 10);
            for (float i = volumeMusic; i > 0; i -= step)
            {
                audioSourceMusique.volume = i;
                yield return new WaitForSeconds(0.1f);
            }
            audioSourceMusique.volume = volumeMusic;
        }

        audioSourceMusique.Stop();
        audioSourceMusique.clip = music;
        audioSourceMusique.Play();
    }

    public static AudioPlay Instance;
    private void Awake()
    {
        if (AudioPlay.Instance != null) return;
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        audioSourceMusique.loop = true;
    }


    public void NoMusic(Toggle toggle)
    {
        EnableMusic(toggle.isOn);
    }

    public void NoSound(Toggle toggle)
    {
        EnableSfx(toggle.isOn);
    }

}
