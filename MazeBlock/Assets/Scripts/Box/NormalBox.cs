using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBox : BasicMovement
{

    public event System.Action OnMove;
    public event System.Action OnFall;

    protected override void StartMoving_Act ()
    {
        base.StartMoving_Act ();
        if (OnMove != null)
            OnMove ();
    }

    protected override void OnFall_Act ()
    {
        if (OnFall != null) {
            OnMove = null;
            OnFall ();
        }
    }
}
