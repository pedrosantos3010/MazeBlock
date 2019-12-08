#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class TilerOptions
{

    public static Transform target;
    public static Vector3 gridOffset;

    public static float GridWidth {
        get { return EditorPrefs.GetFloat ("GridWidth", 1); }
        set {
            if (value == GridWidth)
                return;

            EditorPrefs.SetFloat ("GridWidth", ToOneDecimal (value));
        }
    }

    public static float GridHeight {
        get { return EditorPrefs.GetFloat ("GridHeight", 1); }
        set {
            if (value == GridHeight)
                return;

            EditorPrefs.SetFloat ("GridHeight", ToOneDecimal (value));
        }
    }

    public static Color GridColor {
        get {
            Color _color = new Color (EditorPrefs.GetFloat ("Grid_r"),
                                      EditorPrefs.GetFloat ("Grid_g"),
                                      EditorPrefs.GetFloat ("Grid_b"),
                                      EditorPrefs.GetFloat ("Grid_a")
                           );

            return _color;
        }
        set {
            if (value == GridColor)
                return;

            EditorPrefs.SetFloat ("Grid_r", value.r);
            EditorPrefs.SetFloat ("Grid_g", value.g);
            EditorPrefs.SetFloat ("Grid_b", value.b);
            EditorPrefs.SetFloat ("Grid_a", value.a);
        }
    }

    public static bool ShowGrid {
        get { return EditorPrefs.GetBool ("ShowGrid", true); }
        set {
            if (value == ShowGrid)
                return;
            
            if (value)
                CreateGrid ();
            else
                DestroyGrid ();

            SceneView.RepaintAll ();
            EditorPrefs.SetBool ("ShowGrid", value);
        }
    }

    public static bool ShowTileSelection {
        get { return EditorPrefs.GetBool ("ShowTileSelection", true); }
        set {
            if (value == ShowTileSelection)
                return;

            EditorPrefs.SetBool ("ShowTileSelection", value);
        }
    }

    public static int TileSetsAmount {
        get{ return EditorPrefs.GetInt ("TileSetsAmount", 0); }
        set { 
            EditorPrefs.SetInt ("TileSetsAmount", value);
        }
    }

    static List<TileSet> _allTileSets = new List<TileSet> ();

    public static TileSet GetTileSet (int t_index)
    {
        try {
            TileSet _t = _allTileSets [t_index];
        } catch {
            string path = EditorPrefs.GetString ("TileSetPath" + t_index);
            TileSet _t = AssetDatabase.LoadAssetAtPath (path, typeof(TileSet)) as TileSet;
            _allTileSets.Insert (t_index, _t);
        }

        return _allTileSets [t_index];
    }

    public static void SetTileSet (int t_index, TileSet _tileSet)
    {
        if (_allTileSets [t_index] == _tileSet)
            return;
        
        try {
            string path = AssetDatabase.GetAssetPath (_tileSet.GetInstanceID ());
            EditorPrefs.SetString ("TileSetPath" + t_index, path);
            _allTileSets.Insert (t_index, _tileSet);

            if (t_index >= TileSetsAmount) {
                TileSetsAmount++;
            }
        } catch {
            EditorPrefs.SetString ("TileSetPath" + t_index, "");
            _allTileSets.RemoveAt (t_index);

            if (t_index + 1 >= TileSetsAmount) {
                TileSetsAmount--;
            }
        }
    }

    static TileSet c_tileSet;

    public static int CurrentTileSetIndex {
        get { return EditorPrefs.GetInt ("CurrentTileSetIndex", 0); }
        set {
            if (value == CurrentTileSetIndex)
                return;

            EditorPrefs.SetInt ("CurrentTileSetIndex", value);
            CurrentTileSet = GetTileSet (value);
        }
    }

    public static TileSet CurrentTileSet {
        get {
            if (c_tileSet == null) {
                string path = EditorPrefs.GetString ("TileSetPath" + CurrentTileSetIndex);
                c_tileSet = AssetDatabase.LoadAssetAtPath (path, typeof(TileSet)) as TileSet;
			}
            return c_tileSet;
        }
        set {
            if (c_tileSet != value)
                c_tileSet = value;
            
            string path = AssetDatabase.GetAssetPath (c_tileSet.GetInstanceID ());
            EditorPrefs.SetString ("TileSetPath", path);
        }
    }

    //FUNCTIONS ---------------------------------

    static float ToOneDecimal (float _value)
    {
        return Mathf.Round (_value * 10) / 10;
    }

    public static void CreateGrid ()
    {
        if (GameObject.Find ("GridObject") != null || !ShowGrid)
            return;

        GameObject _gridObject = new GameObject ();
        _gridObject.AddComponent (typeof(GridObject));
        _gridObject.name = "GridObject";
    }

    public static void DestroyGrid ()
    {
        if (GameObject.Find ("GridObject") != null)
            Editor.DestroyImmediate (GameObject.Find ("GridObject"));
    }
}
#endif