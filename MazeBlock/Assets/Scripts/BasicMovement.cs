using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [SerializeField] private bool hasGravity;
    [SerializeField] private LayerMask layerMask;
    private bool moving;
    Vector3 direction;

    //GETSET -------------------------------------
    public bool Moving { get { return moving; } }

    void Start ()
    {
        moving = false;
    }

    public bool CanMove (Vector3 _direction)
    {   
        if (moving)
            return false;

        direction = _direction; 
        if (!GoingPlaceIsEmpty () || !NewPositionHasGround ())
            return false;

        return true;
    }

    public void Move ()
    {
        StartCoroutine (MoveActions ());
    }

    bool GoingPlaceIsEmpty ()
    {
        RaycastHit hittedBox;
        Ray r_posiToDir = new Ray (transform.position, direction);
        bool isEmpty = !Physics.Raycast (r_posiToDir, out hittedBox, MyData.rayDistance, layerMask);

        if (!isEmpty) {
            return CanMoveBox (hittedBox.transform, direction);
		}

        return true;
    }

    bool NewPositionHasGround ()
    {
        RaycastHit hittedGround;
        Ray r_newPosiToGround = new Ray (transform.position + direction, -Vector3.up);
        bool hasGround = Physics.Raycast (r_newPosiToGround, out hittedGround, MyData.rayDistance, layerMask);

        if (hasGround) {
            GroundInteraction_Act (hittedGround.transform);
            return hasGround;
		}

        return (transform.position.y == 1) ? hasGravity : false;
    }

    bool FallNow ()
    {
        if (!hasGravity || transform.position.y != 1)
            return false;

        Ray r_PosiToGround = new Ray (transform.position, -Vector3.up);
        bool shouldFall = !Physics.Raycast (r_PosiToGround, MyData.rayDistance, layerMask);

        if (shouldFall) {
            OnFall_Act ();
		}

        return shouldFall;
    }

    IEnumerator MoveActions ()
    {
        StartMoving_Act ();
        {
            yield return StartCoroutine (LerpMoving (direction));
        }
        EndMoving_Act ();
    }

    IEnumerator LerpMoving (Vector3 _direction)
    {        
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + _direction; 

        TimeToPercent _timeToPercent = new TimeToPercent (Time.time);
        float percentage = 0;

        while (percentage < 1.0f)
        {
            percentage = _timeToPercent.GetPercentage ();
            transform.position = Vector3.Lerp (startPosition, endPosition, percentage);

            yield return null;
        }
    }

    protected virtual void StartMoving_Act ()
    {
        moving = true;
        TimeManager.StartTime ();
        if (direction.y != 0)
            return;
    }

    protected virtual void EndMoving_Act ()
    {
        moving = false;
        TimeManager.EndTime ();

        if (hasGravity && FallNow ())
        {
            direction = -Vector3.up;
            Move ();
		}
    }

    protected virtual bool CanMoveBox (Transform _hittedBox, Vector3 _direction)
    {
        return false;
    }

    protected virtual void  GroundInteraction_Act (Transform _hittedGround)
    {
        if (_hittedGround.CompareTag (MyTags.slipperyGround))
            OnSlide ();
    }

    protected void OnSlide ()
    {
        StartCoroutine (SlideOnTheGround ());
    }

    IEnumerator SlideOnTheGround ()
    {
        yield return new WaitForEndOfFrame ();
        yield return new WaitUntil (() => !TimeManager.timeIsMoving);
        yield return new WaitForEndOfFrame ();

        TimeManager.StartTime ();
        {
            yield return StartCoroutine (WaitStopMoving ());

            if (CanMove (direction))
                Move ();
        }
        TimeManager.EndTime ();
    }

    public IEnumerator WaitStopMoving ()
    {
        yield return null;
        yield return new WaitUntil (() => !this.moving);
        yield return null;
    }

    protected virtual void OnFall_Act ()
    {
    }
}