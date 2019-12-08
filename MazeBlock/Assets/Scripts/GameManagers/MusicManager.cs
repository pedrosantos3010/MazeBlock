using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]

public class MusicManager : MonoBehaviour
{

    static MusicManager _instance;

    public static MusicManager Instance {
        get {
            return _instance;
        }
    }

    public AudioClip[] song;
    public AudioClip levelComplete_sg;
    private AudioSource m_source;

    public bool musicEnabled = true;
    public bool sfxEnabled = true;

    float musicTime = 0;
    float lastMusicTime = 0;
    float unfocusedTime = 0;

    float unscaledTimeInternal;

    bool isInFocus;

    // Use this for initialization
    void Start ()
    {
        isInFocus = true;
        _instance = this;
        m_source = GetComponent<AudioSource> ();

        sfxEnabled = (PlayerPrefs.GetInt ("SFX Volume", 1) == 1) ? true : false;
        musicEnabled = (PlayerPrefs.GetInt ("Music Volume", 1) == 1 ? true : false);
        if (musicEnabled)
            StartCoroutine (UpdateMusic ());
    }

    void OnEnable ()
    {
        GameManager.Instance.LoadActions += WinningSong;
    }

    void OnDisable ()
    {
        GameManager.Instance.LoadActions -= WinningSong;
    }

    public void WinningSong ()
    {
        if (!musicEnabled)
            return;

        m_source.PlayOneShot (levelComplete_sg, m_source.volume / 1.5f);
    }

    public void ChangeMusicConfig ()
    {
        if (musicEnabled) {
            PlayerPrefs.SetInt ("Music Volume", 0);
            musicEnabled = false;
            m_source.Stop ();
		} else {
            PlayerPrefs.SetInt ("Music Volume", 1);
            musicEnabled = true;
            StartCoroutine (UpdateMusic ());
		}
    }

    public void ChangeSFXConfig ()
    {
        sfxEnabled = !sfxEnabled;
        PlayerPrefs.SetInt ("SFX Volume", (sfxEnabled ? 1 : 0));
    }

    public void PlaySFX (AudioClip sfx, float volume = 0.75f)
    {
        if (!sfxEnabled)
            return;

        m_source.PlayOneShot (sfx, volume);
    }

    IEnumerator UpdateMusic ()
    {

        int currentSong = 0;

        while (musicEnabled) {

            {
                int cs = currentSong;
                while (cs == currentSong) {
                    cs = Random.Range (0, song.Length);
				}
                currentSong = cs;
            }

            m_source.PlayOneShot (song [currentSong]);

            #if UNITY_EDITOR
            {//Just a debug
                Debug.Log ("<color=blue>Playing now: song" + (currentSong + 1).ToString () + "</color>");

                Debug.Log ("<color=black>Music Time between loops " +
                (musicTime).ToString () + "</color>");

                lastMusicTime = Time.unscaledTime;
                musicTime = 0;
            }
            #endif

            yield return new WaitUntil (() => (musicTime > (36.9f)));
		}
    }

    //---------------------------------------MUSIC SYNC TIME--------------------------------------------\\
    void FixedUpdate ()
    {
        musicTime = Time.unscaledTime - lastMusicTime;

        if (GetComponentInChildren<UnityEngine.UI.Text> () != null)
            GetComponentInChildren<UnityEngine.UI.Text> ().text = string.Format ("{0:0.0}", musicTime);
    }

    void OnApplicationFocus (bool hasFocus)
    {
        if (isInFocus)
            return;

        StartCoroutine (FixTimeAfterUpdate ());
    }

    IEnumerator FixTimeAfterUpdate ()
    {
        yield return new WaitForFixedUpdate ();

        #if UNITY_EDITOR
        {
            Debug.Log ("<color=cyan>Music time was been fixed</color>");
            Debug.LogWarning ("GAME RESUMED: " + Time.unscaledTime);
        } 
        #endif

        isInFocus = true;
        unfocusedTime = Time.unscaledTime - unfocusedTime;

        lastMusicTime += unfocusedTime;
        musicTime = Time.unscaledTime - lastMusicTime;
    }

    void OnApplicationPause (bool pauseStatus)
    {
        if (!isInFocus)
            return;

        isInFocus = false;
        unfocusedTime = Time.unscaledTime;
      
        #if UNITY_EDITOR
        {
            Debug.LogWarning ("GAME PAUSED: " + unfocusedTime);
        }
        #endif
    }
}
