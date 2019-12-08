#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class GridObject : MonoBehaviour {
    public Camera _camera;

    void Start () {
        if (Application.isPlaying)
            DrawGrid(_camera, Vector3.up, Color.gray);
	}
	
	void OnDrawGizmos () {
        if (Camera.current != null && TilerOptions.ShowGrid && !Application.isPlaying)
        {
            if (TilerOptions.target != null)
                TilerOptions.gridOffset = TilerOptions.target.position;
            else
                TilerOptions.gridOffset = Vector3.zero;

            DrawGrid(Camera.current, TilerOptions.gridOffset, TilerOptions.GridColor);
        }
	}

	void DrawGrid (Camera _camera, Vector3 gridOffset, Color color) {
        //float _width = TilerOptions.GridWidth;
        //float _height = TilerOptions.GridHeight;
        float _size = 1;
		Vector3 g_offset = new Vector3(1 / 2.0f, 0, _size / 2.0f) + gridOffset;

		Vector3 camPos = _camera.transform.position;
		float camOrthoSize = _camera.orthographicSize;
		float zSpacing = (_camera.pixelHeight / _size) + g_offset.z;
		float xSpacing = (_camera.pixelWidth / _size) + g_offset.x;

		Vector3 start;
		Vector3 end;

		for (float z = camPos.z - zSpacing; z < camPos.z + zSpacing; z += _size) {
			if (_size <= 0.1)
				break;

			start = new Vector3(camPos.x - xSpacing, -0.4f, SnapAxis(z, _size) + g_offset.z);
			end = new Vector3(camPos.x + xSpacing, -0.4f, SnapAxis(z, _size) + g_offset.z);

            CreateLine(start, end, color);
        }

		for (float x = camPos.x - xSpacing; x < camPos.x + xSpacing; x += _size) {
			if (_size <= 0.1)
				break;

			start = new Vector3(SnapAxis(x, _size) + g_offset.x, -0.5f, camPos.z - zSpacing);
			end = new Vector3(SnapAxis(x, _size) + g_offset.x, -0.5f, camPos.z + zSpacing);

            CreateLine(start, end, color);
        }
	}

    void CreateLine(Vector3 from, Vector3 to, Color color)
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(from, to);
        } 
        else
        {
            GameObject _go = new GameObject();
            LineRenderer _line = _go.AddComponent<LineRenderer>();
            _line.SetPosition(0, from);
            _line.SetPosition(1, to);
            _line.widthMultiplier = 0.2f;
        }
    }

	float SnapAxis (float _p, float _snapValue) {
		return Mathf.Floor(_p / _snapValue) * _snapValue;
	}
}
#endif