using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager m_reference;

    public static GameManager Instance {
        get {
            if (m_reference == null)
                m_reference = FindObjectOfType<GameManager> ();
			
            return m_reference;
        }
    }

    public event System.Action LoadActions;

    public World c_world;
    public int c_level;

    public int totalCompleteLevels;

    void Start ()
    {
        DontDestroyOnLoad (this.gameObject);
        if (SceneManager.GetActiveScene ().name == "Persistent")
            SceneManager.LoadScene ("Menu"); 
    }

    public IEnumerator RestartLevel ()
    {
        yield return new WaitForSeconds (0.1f);

        TimeManager.EndAllActions ();
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    public void LoadSpecificLevel (int l_number)
    {
        string l_name = "W" + c_world.number.ToString () + "L" + l_number;
        c_level = l_number;
        SceneManager.LoadScene (l_name);
    }

    public void CompleteLevel ()
    {
        #if UNITY_EDITOR
        if (c_world == null)
            return;
        #endif

        c_world.SaveLevel (c_level);

        TimeManager.EndAllActions ();
        if (c_level < c_world.levelCount) {

            c_level++;

            if (LoadActions != null)
                LoadActions ();

            #if UNITY_EDITOR
            {
                string level_W_N = (" level " + c_level.ToString () + " of world " + c_world.number.ToString ());
                Debug.Log ("<color=green> loading" + level_W_N + "</color>");
            }
            #endif

            StartCoroutine (WaitToLoad (c_level));
		} else {
            LoadMenu ();
		}
    }

    public void LoadMenu ()
    {
        StartCoroutine (WaitToLoad ());
    }

    IEnumerator WaitToLoad ()
    {//LoadMenu
        yield return new WaitForSeconds (0.12f);
        SceneManager.LoadScene ("Menu");
    }

    IEnumerator WaitToLoad (int l_number)
    {
        yield return new WaitForSeconds (0.12f);
        string levelName = "W" + c_world.number.ToString () + "L" + l_number.ToString ();
        SceneManager.LoadScene (levelName);
    }

}
