using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    public Text pressText;
    public Text loadingText;

    public Image loadingImage;

    private MusicManager musicManager;

    private void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    void Start()
    {
        pressText.enabled = false;

        StartCoroutine(AsynchronousLoad("MainScene"));

        StartCoroutine(ScaleText());

        if (musicManager)
            StartCoroutine(musicManager.MusicFadeIn(musicManager.musicPlayer));
    }

    private IEnumerator AsynchronousLoad(string scene)
    {
        yield return null;

        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);

            loadingImage.fillAmount = progress;

            if (async.progress == 0.9f)
            {
                pressText.enabled = true;

                loadingText.text = "Loading completed";

                if (Input.anyKey)
                {
                    if (musicManager)
                        musicManager.PlaySound(musicManager.buttonInterfaceSound);

                    async.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }

    private IEnumerator ScaleText()
    {
        WaitForSeconds delay = new WaitForSeconds(0.5f);

        while (true)
        {
            pressText.fontSize += 10;
            yield return delay;
            pressText.fontSize -= 10;
            yield return delay;
        }
    }

    private void OnDisable()
    {
        System.GC.Collect();
    }
}
