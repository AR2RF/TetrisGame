using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuInterface : MonoBehaviour
{

    private MusicManager musicManager;

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject highScorePanel;
    public GameObject autorsPanel;
    public GameObject helpPanel;

    public Text highScore;

    private void Awake()
    {
        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
    }

    void Start()
    {
        if (musicManager)
            StartCoroutine(musicManager.MusicFadeIn(musicManager.musicPlayer));

        mainPanel.SetActive(true);
        highScorePanel.SetActive(false);
        autorsPanel.SetActive(false);

        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        }       
    }

    public void BtnStart()
    {
        musicManager.PlaySound(musicManager.buttonInterfaceSound);

        SceneManager.LoadScene("LoadScreen");
    }

    public void BtnHighScore()
    {
        musicManager.PlaySound(musicManager.buttonInterfaceSound);

        mainPanel.SetActive(false);
        highScorePanel.SetActive(true);
    }

    public void BtnAutors()
    {
        musicManager.PlaySound(musicManager.buttonInterfaceSound);

        mainPanel.SetActive(false);
        autorsPanel.SetActive(true);
    }

    public void BtnQuit()
    {
        musicManager.PlaySound(musicManager.buttonInterfaceSound);

        Application.Quit();
    }

    public void BtnHelp()
    {
        musicManager.PlaySound(musicManager.buttonInterfaceSound);

        mainPanel.SetActive(false);
        helpPanel.SetActive(true);
    }

    public void BtnBack()
    {
        musicManager.PlaySound(musicManager.buttonInterfaceSound);

        mainPanel.SetActive(true);
        helpPanel.SetActive(false);
        highScorePanel.SetActive(false);
        autorsPanel.SetActive(false);
    }

    public void BtnMusicOnOff()
    {
        musicManager.MusicOnOff();
    }
}
