using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorConfig : MonoBehaviour {

    static bool showGrid;

    public static void CreateGrid()
    {
        /*
        if (GameObject.Find("GridObject") != null || !showGrid)
            return;

        GameObject _gridObject = new GameObject();
        _gridObject.AddComponent(typeof(GridObject));
        _gridObject.name = "GridObject";
        */
    }

    public static void DestroyGrid()
    {
        //if (GameObject.Find("GridObject") != null)
            //Editor.DestroyImmediate(GameObject.Find("GridObject"));
    }
}
