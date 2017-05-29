using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    [Header("AudioSources")]
    public AudioSource musicPlayer;
    public AudioSource interfaceSounds;

    [Header("AudioClips")]
    public AudioClip buttonInterfaceSound;
    public AudioClip moveSound;
    public AudioClip lineClearedSound;
    public AudioClip LevelUpSound;

    void Start ()
    {
        DontDestroyThisGameObject();
    }
	
    private void DontDestroyThisGameObject()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    public void MusicOnOff()
    {
        PlaySound(buttonInterfaceSound);

        if (AudioListener.volume > 0)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

    public IEnumerator MusicFadeIn(AudioSource source)
    {
        if (!source.isPlaying)
        {
            source.Play();
        }

        while (source.volume < 0.5f)
        {
            source.volume += Mathf.Lerp(0, 1f, 0.1f);
            yield return new WaitForSeconds(0.5f);
        }
        StopCoroutine(MusicFadeIn(source));
    }

    public void PlaySound(AudioClip clip)
    {
        interfaceSounds.PlayOneShot(clip);
    }
}
