using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldButtonManager : MonoBehaviour
{

    WorldButton[] _worldButton;

    // Use this for initialization
    void Awake ()
    {
        _worldButton = GetComponentsInChildren<WorldButton> ();
        UpdateButtons ();
    }

    public void UpdateButtons ()
    {
        #if UNITY_EDITOR
        {
            Debug.Log ("<color=cyan> Updating worldButtons</color>");
        }
        #endif

        foreach (WorldButton _wb in _worldButton) {
            _wb.UpdateButton ();
		}
    }
}
