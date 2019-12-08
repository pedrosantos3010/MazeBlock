using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorInput : MonoBehaviour {
    
	void Start () {
		
	}

	void Update () {
		
	}

    void UpdateCamera ()
    {

    }

    void EditLevel ()
    {

        CreationMode();

    }

    void CreationMode()
    {

    }

    Vector3 SnapPosition(Vector3 _position, float snapValue)
    {
        Vector3 snapedPosition = new Vector3(
                                     Mathf.Round(_position.x / snapValue) * snapValue,
                                     Mathf.Round(_position.y / snapValue) * snapValue,
                                     Mathf.Round(_position.z / snapValue) * snapValue);
        return snapedPosition;
    }
}
