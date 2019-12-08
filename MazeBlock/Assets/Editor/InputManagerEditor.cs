using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor {

	InputManager _im;
	static GameObject levelRoot;
	static Bounds levelBounds;

	void OnEnable () {
		_im = (InputManager)target;
	}

	public override void OnInspectorGUI () {
		base.DrawDefaultInspector ();

		UpdateOffset ();
	}

	void UpdateOffset () {
		if (GameObject.Find ("Level") == null)
			return;

		levelRoot = GameObject.Find ("Level");
		levelBounds = new Bounds ();

		levelBounds.center = levelRoot.transform.position;
		foreach (Renderer _r in levelRoot.GetComponentsInChildren<Renderer>()) {
			if (_r.transform.tag != "StartLevel" && _r.transform.tag != "EndLevel")
				levelBounds.Encapsulate (_r.bounds);
		}

		if (levelBounds.center.z != _im.z_levelOffset) {
			_im.z_levelOffset = levelBounds.center.z;
			EditorUtility.SetDirty (_im);
			UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty ();
		}

		EditorGUILayout.BeginVertical ("Box");
		EditorGUILayout.LabelField ("Input Method");
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button("Pad1")) {
			_im.InputMethod = 0;
		}
		if (GUILayout.Button("Pad2")) {
			_im.InputMethod = 1;
		}
		if (GUILayout.Button("Swipe")) {
			_im.InputMethod = 2;
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
	}
}