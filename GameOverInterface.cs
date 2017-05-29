using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverInterface : MonoBehaviour
{
    private MusicManager musicManager;

    public Text score;
    public Text highScore;

    private void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    private void Start()
    {
        score.text = PlayerPrefs.GetInt("Score").ToString();
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    public void BtnRestart()
    {
        if (musicManager)
            musicManager.PlaySound(musicManager.buttonInterfaceSound);

        SceneManager.LoadScene("LoadScreen");
    }

    public void BtnMainMenu()
    {
        if (musicManager)
            musicManager.PlaySound(musicManager.buttonInterfaceSound);

        SceneManager.LoadScene("MainMenu");
    }
}
