using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class TilerTool : Editor
{
    static int selectedTilerIndex;
    static int selectedPrefabIndex;
    static Vector3 mouseHandlePosition;
    static Vector3 mouseDragStart;
    static bool isDragging;
    static Color handleColor;

    public static int SelectedTool
    {
        get { return EditorPrefs.GetInt("SelectedEditorTool"); }
        set
        {
            if (value == SelectedTool)
                return;

            EditorPrefs.SetInt("SelectedEditorTool", value);

            UpdateToolInformation(value);

        }
    }

    static Scene m_currentScene;

    static TilerTool()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;

        EditorApplication.hierarchyWindowChanged -= OnSceneChanged;
        EditorApplication.hierarchyWindowChanged += OnSceneChanged;

        if (TilerOptions.target == null && GameObject.Find("Level") != null)
            TilerOptions.target = GameObject.Find("Level").transform;

        //SelectedTool = 0;
    }

    void OnDestroy()
    {
        if (GameObject.Find("GridObject") != null)
            DestroyImmediate(GameObject.Find("GridObject"));
        
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        EditorApplication.hierarchyWindowChanged -= OnSceneChanged;
    }

    static void OnSceneChanged()
    {
        if (Application.isPlaying)
            SelectedTool = 0;
        if (m_currentScene == SceneManager.GetActiveScene())
            return;

        m_currentScene = SceneManager.GetActiveScene();
        if (GameObject.Find("Level") != null)
            TilerOptions.target = GameObject.Find("Level").transform;
        else
            SelectedTool = 0;

        if (SelectedTool != 0)
        {
            TilerOptions.CreateGrid();
        }
        else
        {
            TilerOptions.DestroyGrid();
        }
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        DrawSelectionTools(sceneView.position);

        Event e = Event.current;
        CheckToolHotkeys(e);

        if (SelectedTool == 0)
            return;

        HandleUtility.Repaint();

        if (TilerOptions.ShowTileSelection)
            DrawSelectionPrefabs(sceneView.position);

        UpdateMousePosition(e);
        DrawMouseHandle();
        CheckMouseInput(e);
    }

    static void DrawSelectionTools(Rect position)
    {

        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(0, 10, position.width, 40));
        {
            string[] buttonLabels = new string[] { "None", "Paint", "Erase" };
            SelectedTool = GUILayout.SelectionGrid(
                SelectedTool,
                buttonLabels,
                3,
                EditorStyles.toolbarButton,
                GUILayout.Width(300),
                GUILayout.Height(100)
            );
        }
        GUILayout.EndArea();

        Handles.EndGUI(); 

        if (SelectedTool == 1)
            DrawSelecionTiler(position);
    }

    static void DrawSelecionTiler(Rect position)
    {
        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(100, 30, position.width, 40));
        {
            string[] buttonLabels = new string[TilerOptions.TileSetsAmount];

            for (int i = 0; i < TilerOptions.TileSetsAmount; i++)
            {
                buttonLabels[i] = i.ToString();
            }

            TilerOptions.CurrentTileSetIndex = GUILayout.SelectionGrid(
                TilerOptions.CurrentTileSetIndex,
                buttonLabels,
                3,
                EditorStyles.toolbarButton,
                GUILayout.Width(300 / TilerOptions.TileSetsAmount),
                GUILayout.Height(100)
            );
        }
        GUILayout.EndArea();

        Handles.EndGUI(); 
    }

    static void DrawSelectionTiler(Rect _position)
    {
        Handles.BeginGUI();
        Rect boxPosition = new Rect(0, _position.height - 100, _position.width, 100);
        GUI.Box(boxPosition, GUIContent.none, EditorStyles.textArea);

        for (int i = 0; i < TilerOptions.CurrentTileSet.prefabs.Length; i++)
        {
            DrawPrefab(i, _position);
        }
        Handles.EndGUI();
    }

    static void DrawSelectionPrefabs(Rect _position)
    {
        Handles.BeginGUI();
        Rect boxPosition = new Rect(0, _position.height - 100, _position.width, 100);
        GUI.Box(boxPosition, GUIContent.none, EditorStyles.textArea);

        for (int i = 0; i < TilerOptions.CurrentTileSet.prefabs.Length; i++)
        {
            DrawPrefab(i, _position);
        }
        Handles.EndGUI();
    }

    static void DrawPrefab(int index, Rect _position)
    {
        if (TilerOptions.CurrentTileSet == null)
            return;
		
        bool isActive = false;

        if (SelectedTool == 1 && selectedPrefabIndex == index)
            isActive = true;

        if (TilerOptions.CurrentTileSet.prefabs[index] != null)
        {
            Texture2D previewAsset = AssetPreview.GetAssetPreview(
                                         TilerOptions.CurrentTileSet.prefabs[index].gameObject);
            
            GUIContent content = new GUIContent(previewAsset);

            GUI.Label(new Rect(index * 90 + 15, _position.height - 97, 100, 90),
                TilerOptions.CurrentTileSet.prefabs[index].name, EditorStyles.label
            );
            bool isToggle = GUI.Toggle(new Rect(index * 90, _position.height - 80, 75, 75),
                                isActive, content, EditorStyles.toggle);

            if (isToggle)
            {
                selectedPrefabIndex = index;
                SelectedTool = 1;
            }
        }
    }

    static void CheckToolHotkeys(Event e)
    {
        if (e.keyCode == KeyCode.Escape)
            SelectedTool = 0;

        if (!e.isKey || e.isMouse || e.alt)
            return;
		 
        if (e.control)
        {
            if (e.keyCode == KeyCode.Q)
                SelectedTool = 0;
            if (e.keyCode == KeyCode.W)
                SelectedTool = 1;
            if (e.keyCode == KeyCode.E)
                SelectedTool = 2;
        }

        if (SelectedTool != 1)
            return;

        CheckHotNumbers(e);
    }

    static void CheckHotNumbers(Event e)
    {
        if (e.keyCode == KeyCode.R && e.control)
        {
            int c_tsIndex = TilerOptions.CurrentTileSetIndex;

            selectedPrefabIndex = 0;

            if (c_tsIndex < TilerOptions.TileSetsAmount - 1)
            {
                TilerOptions.CurrentTileSetIndex++;
            }
            else
            {
                TilerOptions.CurrentTileSetIndex = 0;
            }
        }

        if (!e.shift)
            return;

        if (e.keyCode == KeyCode.Alpha1) selectedPrefabIndex = 0;
        if (e.keyCode == KeyCode.Alpha2) selectedPrefabIndex = 1;
        if (e.keyCode == KeyCode.Alpha3) selectedPrefabIndex = 2;
        if (e.keyCode == KeyCode.Alpha4) selectedPrefabIndex = 3;
        if (e.keyCode == KeyCode.Alpha5) selectedPrefabIndex = 4;
        if (e.keyCode == KeyCode.Alpha6) selectedPrefabIndex = 5;
    }

    static void UpdateMousePosition(Event e)
    {

        if (e == null)
            return;

        Vector2 _mousePosition = e.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(_mousePosition);
        RaycastHit hit;

        Vector3 g_offset = new Vector3(
                               TilerOptions.gridOffset.x - (int)TilerOptions.gridOffset.x, 0,
                               TilerOptions.gridOffset.z - (int)TilerOptions.gridOffset.z);

        if (Physics.Raycast(ray, out hit))
        {
            mouseHandlePosition = hit.transform.position;
            if (SelectedTool == 1)
                mouseHandlePosition += hit.normal;

        }
        else
        {
            Plane _ground = new Plane(Vector3.up, Vector3.zero);
            float _hitDistance;
            if (_ground.Raycast(ray, out _hitDistance))
            {
                Vector3 hitP = ray.GetPoint(_hitDistance);

                mouseHandlePosition = SnapPosition(hitP - g_offset / 2, 1) + g_offset;
                mouseHandlePosition.y = 0;
            }
        }

        if (e.type == EventType.MouseDown && e.button == 0 && e.shift &&
            !e.control && !e.alt && !isDragging)
        {
            mouseDragStart = mouseHandlePosition;
            isDragging = true;
        }
        if (!e.isMouse && !e.shift && isDragging || e.type == EventType.MouseUp)
        {
            isDragging = false;
        }

    }

    static void DrawMouseHandle()
    {
        Handles.color = handleColor;

        Vector3 _handlePosition = mouseHandlePosition;
        Vector3 _handleSize = Vector3.one;

        if (isDragging)
        {
            _handlePosition = Vector3.Lerp(mouseDragStart, mouseHandlePosition, 0.5f);
            _handlePosition.y = mouseDragStart.y;
            _handleSize = new Vector3(Mathf.Floor(Mathf.Abs(mouseDragStart.x - mouseHandlePosition.x) + 1f), 1,
                Mathf.Floor(Mathf.Abs(mouseDragStart.z - mouseHandlePosition.z) + 1f));
        }

        Handles.DrawWireCube(_handlePosition, _handleSize);
    }

    static void CheckMouseInput(Event e)
    {

        if (!e.isMouse || e.alt)
            return;

        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        if (e.shift)
        {

            if (e.type == EventType.MouseUp && e.button != 1 && e.button == 0)
            {

                if (SelectedTool == 1)
                    AddBlocksInArea();

                if (SelectedTool == 2)
                    RemoveBlocksInArea();
            }

            GUIUtility.hotControl = controlID;
            e.Use();

            return;
        }

        if (e.type == EventType.MouseDown)
        {
            if (e.button == 0)
            {
                GUIUtility.hotControl = controlID;
                e.Use();

                if (SelectedTool == 1)
                {
                    AddBlock(mouseHandlePosition);
                }

                if (SelectedTool == 2)
                    RemoveBlock(mouseHandlePosition);
            }

            if (e.button == 1 && e.control)
            {
                GUIUtility.hotControl = controlID;
                e.Use();

                if (SelectedTool == 1)
                    RotateBlock(e);

                if (SelectedTool == 2)
                {
                    if (TilerOptions.target.GetChild(0) == null)
                    {
                        Debug.Log(null);
                        return;
                    }
                    Undo.DestroyObjectImmediate(TilerOptions.target.GetChild(0).gameObject);
                    UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
                }
            }
        }
    }

    static void AddBlocksInArea()
    {
        int xCount = (int)Mathf.Abs(mouseDragStart.x - mouseHandlePosition.x) + 1;
        int zCount = (int)Mathf.Abs(mouseDragStart.z - mouseHandlePosition.z) + 1;

        for (int x = 0; Mathf.Abs(x) < xCount; x += (int)Mathf.Sign(mouseHandlePosition.x - mouseDragStart.x))
        {
            for (int z = 0; Mathf.Abs(z) < zCount; z += (int)Mathf.Sign(mouseHandlePosition.z - mouseDragStart.z))
            {

                Vector3 blockPosition = new Vector3(x + mouseDragStart.x, mouseDragStart.y, z + mouseDragStart.z);
                Ray ray = new Ray(blockPosition + Vector3.up * 0.75f, Vector3.down);

                if (!Physics.Raycast(ray, 0.75f))
                    AddBlock(blockPosition);
            }
        }
    }

    static void AddBlock(Vector3 _position)
    { 
        Transform _prefab = TilerOptions.CurrentTileSet.prefabs[selectedPrefabIndex];
        GameObject newCube = (GameObject)PrefabUtility.InstantiatePrefab(_prefab.gameObject);
        newCube.transform.position = _position;

        if (TilerOptions.target != null)
        {

            Transform _holder = TilerOptions.target.Find("Holder");
            if (_holder == null)
            {
                _holder = new GameObject("Holder").transform;
                _holder.parent = TilerOptions.target;
            }

            newCube.transform.parent = _holder;
        }

        Undo.RegisterCreatedObjectUndo(newCube, "Add " + _prefab.name);

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
    }

    static void RotateBlock(Event e)
    {
        Vector2 _mousePosition = e.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(_mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            float newRotation = hit.transform.eulerAngles.y + 90;
            hit.transform.eulerAngles = new Vector3(0,
                Mathf.Round(newRotation / 90) * 90, 0);

            Undo.RegisterCompleteObjectUndo(hit.transform.gameObject, "Rotated Object");

            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }
    }

    static void RemoveBlocksInArea()
    {
        int xCount = (int)Mathf.Abs(mouseDragStart.x - mouseHandlePosition.x) + 1;
        int zCount = (int)Mathf.Abs(mouseDragStart.z - mouseHandlePosition.z) + 1;

        for (int x = 0; Mathf.Abs(x) < xCount; x += (int)Mathf.Sign(mouseHandlePosition.x - mouseDragStart.x))
        {
            for (int z = 0; Mathf.Abs(z) < zCount; z += (int)Mathf.Sign(mouseHandlePosition.z - mouseDragStart.z))
            {

                Vector3 blockPosition = new Vector3(x + mouseDragStart.x, mouseDragStart.y, z + mouseDragStart.z);
                RemoveBlock(blockPosition);
            }
        }
    }

    static void RemoveBlock(Vector3 m_position)
    {
        Ray ray = new Ray(m_position + Vector3.up * 0.75f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.75f))
        {
            Undo.DestroyObjectImmediate(hit.transform.gameObject);

            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }
    }

    static void UpdateToolInformation(int _value)
    {
        switch (_value)
        {
            case (0):
                TilerOptions.DestroyGrid();
                isDragging = false;
                mouseDragStart = Vector3.zero;
                Tools.hidden = false;
                break;

            case (1):

                if (TilerOptions.CurrentTileSet == null)
                {
                    Debug.LogError("There is no tile set to use. Try to choose one in Tiler Window");
                    break;
                }

                TilerOptions.CreateGrid();
                Tools.hidden = true;
                handleColor = Color.yellow;
                break;

            case (2):

                if (TilerOptions.CurrentTileSet == null)
                {
                    Debug.LogError("There is no tile set to use. Try to choose one in Tiler Window");
                    return;
                }

                TilerOptions.CreateGrid();
                Tools.hidden = true;
                handleColor = Color.red;
                break;

            default:
                Tools.hidden = false;
                break;
        }
    }

    static Vector3 SnapPosition(Vector3 _position, float snapValue)
    {
        Vector3 snapedPosition = new Vector3(
                                     Mathf.Round(_position.x / snapValue) * snapValue,
                                     Mathf.Round(_position.y / snapValue) * snapValue,
                                     Mathf.Round(_position.z / snapValue) * snapValue);
        return snapedPosition;
    }
}
