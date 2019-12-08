using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyBox : NormalBox
{

    bool stopSlide = false;

    protected override bool CanMoveBox (Transform _hittedBox, Vector3 _direction)
    {
        if (!_hittedBox.CompareTag (MyTags.slipperyBox))
            return false;

        BasicMovement _slipBox = _hittedBox.GetComponent<BasicMovement> ();
        if (_slipBox.CanMove (_direction))
            _slipBox.Move (); 
	 
        return base.CanMoveBox (_hittedBox, _direction);//false
    }

    protected override void EndMoving_Act ()
    {
        base.EndMoving_Act ();
     
        if (stopSlide) {
            stopSlide = false;
            return;
        }

        base.OnSlide ();
    }

    protected override void GroundInteraction_Act (Transform _hittedGround)
    {
    }

    void OnTriggerEnter (Collider _col)
    {
        if (_col.CompareTag ("BoxPusher"))
            stopSlide = true;
    }

}