using System.Collections;
using UnityEngine;

public class BoxPusher : MonoBehaviour
{

    void OnDrawGizmos ()
    {
        Gizmos.color = Color.blue;

        Vector3 startPosition = transform.position + Vector3.up * 1.5f;
        Vector3 direction = transform.forward;

        DebugExtension.DrawArrow (startPosition, direction, Gizmos.color);
    }

    void OnTriggerEnter (Collider _col)
    {
        BasicMovement _move = _col.GetComponent<BasicMovement> (); 
        if (_move != null) {
            StartCoroutine (PushBox (_move)); 
        }
    }

    IEnumerator PushBox (BasicMovement _box)
    {
        TimeManager.StartTime ();
        {
            yield return new WaitUntil (() => !_box.Moving);

            if (_box.CanMove (transform.forward)) {
                _box.Move ();
		    }
        }
        TimeManager.EndTime ();
    }
}