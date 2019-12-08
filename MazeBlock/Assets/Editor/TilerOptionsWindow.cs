using System.Collections;
using UnityEngine;
using UnityEditor;

public class TilerOptionsWindow : EditorWindow
{
    [MenuItem ("Window/Tiler Configurations")]
    static void OnTilerConfigurations ()
    {
        TilerOptionsWindow _tow = EditorWindow.GetWindow<TilerOptionsWindow> ("Tiler Options");
        _tow.minSize = new Vector2 (300, 300);
    }

    bool _editGrid = true;
    bool _changeTileSets = true;

    void OnGUI ()
    {

        FormatHeader ();

        TilerOptions.ShowGrid = EditorGUILayout.Toggle ("Show Grid", TilerOptions.ShowGrid);

        _editGrid = EditorGUILayout.Foldout (_editGrid, "Edit Grid");
        if (_editGrid) {
            GridOptions ();
		}

        TilerOptions.target = EditorGUILayout.ObjectField ("Target", TilerOptions.target,
                                                           typeof(Transform), true) as Transform;

        _changeTileSets = EditorGUILayout.Foldout (_changeTileSets, "Edit TileSets");
        if (_changeTileSets) {
            DrawAllTileSets ();
        }

//        TileSet _ts = DrawTileSet (TilerOptions.CurrentTileSetIndex);
//
//        if (_ts != TilerOptions.CurrentTileSet) {
//            TilerOptions.CurrentTileSetIndex = _ts;
//        }
    }

    void DrawAllTileSets ()
    {
        EditorGUI.indentLevel += 1;
        {
            for (int i = 0; i <= TilerOptions.TileSetsAmount; i++) {

                EditorGUI.BeginChangeCheck ();
                TileSet _ts = DrawTileSet (i);

                if (EditorGUI.EndChangeCheck ()) {
                    TilerOptions.SetTileSet (i, _ts);
                }
            }
        }
        EditorGUI.indentLevel -= 1;

        EditorGUILayout.Space ();
    }

    TileSet DrawTileSet (int tileNumber)
    {
        return EditorGUILayout.ObjectField ("Tile Set", TilerOptions.GetTileSet (tileNumber),
                                            typeof(TileSet), true) as TileSet;
    }

    void FormatHeader ()
    {
        EditorGUILayout.BeginVertical ("Box");
        {
            EditorGUILayout.LabelField ("Edit Tiler Options", EditorStyles.boldLabel);
        }
        EditorGUILayout.EndVertical ();
    }

    void GridOptions ()
    {
        EditorGUI.indentLevel += 1;
        {
            TilerOptions.GridWidth = DrawSlider ("Grid Width", TilerOptions.GridWidth);
            TilerOptions.GridHeight = DrawSlider ("Grid Height", TilerOptions.GridHeight);

            TilerOptions.GridColor = EditorGUILayout.ColorField ("Grid Color", TilerOptions.GridColor);
        }
        EditorGUI.indentLevel -= 1;
        EditorGUILayout.Space ();
    }

    float DrawSlider (string label, float value)
    {
        return EditorGUILayout.Slider (label, value, 0, 5);
    }
}