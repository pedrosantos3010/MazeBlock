using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class LevelButtonManager : MonoBehaviour {

	private LevelButton[] _levelButton;
	public Text t_number;
	public Text t_score;
	public Renderer labelRenderer;

	// Use this for initialization
	void Awake () {
		_levelButton = GetComponentsInChildren<LevelButton> ();
	}

	public void UpdateWorldLabel(int w_number, string w_score, Color w_color) {
		t_number.text = w_number.ToString ();
		t_score.text = w_score;
		labelRenderer.material.color = w_color;
	}
	
	public void UpdateButtons (int page) {
		for (int i = 0; i < _levelButton.Length; i++) {
			if (page * 12 + i < GameManager.Instance.c_world.levelCount)
				_levelButton [i].UpdateButton (page * 12 + i);
			else
				_levelButton [i].DeactivateButton ();
		}
	}
}
