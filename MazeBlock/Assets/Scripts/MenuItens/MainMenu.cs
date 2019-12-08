using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    #if UNITY_EDITOR
    void OnEnable ()
    {
        if (GameManager.Instance == null)
            UnityEngine.SceneManagement.SceneManager.LoadScene ("Persistent");
    }
    #endif

    void Awake ()
    {
        GameManager.Instance.totalCompleteLevels = 0;
    }

    // Use this for initialization
    void Start ()
    {
        CameraSmooth.Instance.SetTargetImmediate (GameObject.Find ("Menu").transform);
        GameObject.Find ("WorldSelection").SetActive (false);
        GameObject.Find ("LevelSelection").SetActive (false);
        GameObject.Find ("InfoMenu").SetActive (false);
        GameObject.Find ("ConfigMenu").SetActive (false);
    }

    void Update ()
    {
        if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown ("e")) {
            #if UNITY_EDITOR
            {
                Debug.LogWarning ("Data has been restarted");
            }
            #endif
            GameManager.Instance.totalCompleteLevels = 0;
            PlayerPrefs.DeleteAll ();

            WorldButtonManager _wbm = FindObjectOfType<WorldButtonManager> ();
            _wbm.UpdateButtons ();
		}
    }
}
