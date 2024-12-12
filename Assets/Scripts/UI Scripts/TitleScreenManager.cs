using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
public class TitleScreenManager : MonoBehaviour
{

    public GameObject titleScreenUI;

    public GameObject howToPlayUI;
    // Start is called before the first frame update
    void Start()
    {
        titleScreenUI.SetActive(true);
        howToPlayUI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void HowToPlay()
    {
        titleScreenUI.SetActive(false);
        howToPlayUI.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void CloseHowToPlay()
    {
        titleScreenUI.SetActive(true);
        howToPlayUI.SetActive(false);
    }
}
