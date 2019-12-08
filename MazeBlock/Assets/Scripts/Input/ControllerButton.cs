using System.Collections;
using UnityEngine;

public class ControllerButton : MonoBehaviour, IButton
{

    PlayerController p_controller;
    public Vector3 direction;

    public float scaleModifier = 0.2f;
    Vector3 m_scale;
    Vector3 m_scale2;

    void Start ()
    {
        p_controller = FindObjectOfType<PlayerController> () as PlayerController;

        m_scale = transform.localScale;
        m_scale2 = m_scale - Vector3.one * scaleModifier;
    }

    public void OnButtonInteraction ()
    {
        if (TimeManager.timeIsMoving) {
            
            #if UNITY_EDITOR 
            {
                Debug.LogWarning ("Time Is Moving");
            }
            #endif

            return;
		}

        StartCoroutine (MoveLoop ());
        StartCoroutine (HoldAnim ());
    }

    IEnumerator MoveLoop ()
    {
        bool firstTime = true;
        WaitForSeconds _waitLerpTime = new WaitForSeconds (TimeManager.LerpSpeed);

        do {
            p_controller.SetInput (direction);

            if (firstTime) {
                firstTime = false;
                yield return _waitLerpTime;
			}

            yield return new WaitUntil (() => !TimeManager.timeIsMoving);
            yield return _waitLerpTime;

		} while (Input.GetMouseButton (0) || Input.touchCount != 0);
    }

    IEnumerator HoldAnim ()
    {

        float startTime = Time.time;
        float durationTaken = TimeManager.LerpSpeed / 2;
        float percent = 0;

        while (percent < 1) {

            percent = (Time.time - startTime) / durationTaken;
            transform.localScale = Vector3.Lerp (m_scale, m_scale2, percent);
			
            yield return null;
		}

        #if UNITY_EDITOR
        {
            yield return new WaitUntil (() => (Input.GetMouseButtonUp (0)));
        }
        #elif UNITY_ANDROID
        {
            yield return new WaitUntil (() => (Input.touchCount == 0));
        }
        #endif

        StartCoroutine (ReleaseAnim ());
    }

    IEnumerator ReleaseAnim ()
    {
        float startTime = Time.time;
        float durationTaken = TimeManager.LerpSpeed / 2;
        float percent = 0;

        while (percent < 1) {

            percent = (Time.time - startTime) / durationTaken;
//			float interpolation = (-Mathf.Pow (percent, 2) + percent) * 4;

            transform.localScale = Vector3.Lerp (m_scale2, m_scale, percent);

            yield return null;
		}
    }
}
