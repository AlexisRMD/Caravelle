using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menuprincipal;
    public GameObject menucredentials;

    public void LaunchGame()
    {
        AudioPlay.Instance.PlayOneShot(AudioPlay.Instance.button);
        SceneManager.LoadScene("MainScene");
    }

    public void Quitter()
    {
        AudioPlay.Instance.PlayOneShot(AudioPlay.Instance.button);
        Application.Quit();
    }

    public void GoCredit()
    {
        AudioPlay.Instance.PlayOneShot(AudioPlay.Instance.button);
        menuprincipal.SetActive(false);
        menucredentials.SetActive(true);
    }
    public void GoMenu()
    {
        AudioPlay.Instance.PlayOneShot(AudioPlay.Instance.button);
        menucredentials.SetActive(false);
        menuprincipal.SetActive(true);
    }
}
