using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour {

    private MusicManager musicManager;
    private Tetrimino tetrimino;
    public GameController gameController;

    public Button[] controlButtons;

    public Text score;
    public Text highScore;
    public Text level;

    private void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    private void Start()
    {
        if(musicManager)
        StartCoroutine(musicManager.MusicFadeIn(musicManager.musicPlayer));

        score.text = gameController.currentScore.ToString();
        highScore.text = gameController.highScore.ToString();
        level.text = gameController.currentLevel.ToString();
    }

    public void BtnPause()
    {
        if (musicManager)
            musicManager.PlaySound(musicManager.buttonInterfaceSound);

        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;

            HideTetriminos();

            if (musicManager)
                musicManager.musicPlayer.volume = musicManager.musicPlayer.volume / 10;
        }
        else
        {
            Time.timeScale = 1f;

            ShowTetriminos();

            if (musicManager)
                musicManager.musicPlayer.volume = musicManager.musicPlayer.volume * 10;
        }
    }

    public void BtnMainMenu()
    {
        if (musicManager)
            musicManager.PlaySound(musicManager.buttonInterfaceSound);

        if (Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }
            SceneManager.LoadScene("MainMenu");
    }

    private void HideTetriminos()
    {
        foreach (Transform tetrimino in gameController.transform)
        {
            foreach (Transform mino in tetrimino)
            {
                mino.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    private void ShowTetriminos()
    {
        foreach (Transform tetrimino in gameController.transform)
        {
            foreach (Transform mino in tetrimino)
            {
                mino.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
}
