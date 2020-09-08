using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BasicMovement
{

    void Start ()
    {
        try
        {
            Vector3 startPosition = GameObject.FindGameObjectWithTag(MyTags.startLevel).transform.position;
            startPosition.y = 1;
            transform.position = startPosition;
        }
        catch
        {
            
        }

        SwipeController.OnSwipe += SwipeInput;
    }

    void OnDisable ()
    {
        SwipeController.OnSwipe -= SwipeInput;
    }

    void SwipeInput (SwipeDirection _direction)
    {
        switch (_direction) {
            case SwipeDirection.Up:
                SetInput (Vector3.forward);
                break;
            case SwipeDirection.Left:
                SetInput (Vector3.left);
                break;
            case SwipeDirection.Down:
                SetInput (-Vector3.forward);
                break;
            case SwipeDirection.Right:
                SetInput (-Vector3.left);
                break;
		}
    }

    public void SetInput (Vector3 direction)
    {
        if (TimeManager.timeIsMoving)
            return;
        
        if (CanMove (direction))
            Move ();
    }

    #if UNITY_EDITOR
    void Update ()
    {

        if (Input.GetKeyDown ("w"))
            SetInput (Vector3.forward);
        
        if (Input.GetKeyDown ("a"))
            SetInput (Vector3.left);

        if (Input.GetKeyDown ("s"))
            SetInput (-Vector3.forward);
        
        if (Input.GetKeyDown ("d"))
            SetInput (-Vector3.left);

        if (Input.GetKeyDown ("r"))
            InputManager.Instance.ResetGame ();
    }
    #endif

    //    protected override IEnumerator SlideOnTheGround ()
    //    {
    //        TimeManager.StartTime ();
    //        {
    //            yield return StartCoroutine (MyFunctions.WaitTimeNotMove ());
    //            base.SlideOnTheGround ();
    //        }
    //        TimeManager.EndTime ();
    //    }

    protected override bool CanMoveBox (Transform _hittedBox, Vector3 _direction)
    {
        BasicMovement box = _hittedBox.GetComponent<BasicMovement> ();
        if (box == null)
            return false;

        if (box.Moving)
            return true;

        if (box.CanMove (_direction)) {
            box.Move ();
            return true;
		}

        return false;
    }

    void OnTriggerEnter (Collider _col)
    {
        if (_col.CompareTag (MyTags.endLevel)) {
            #if UNITY_EDITOR
            {
                Debug.Log ("<color=green>Level Complete</color>");
                if (GameManager.Instance == null)
                    return;
            }
            #endif

            GameManager.Instance.CompleteLevel ();
		}
    }
}
