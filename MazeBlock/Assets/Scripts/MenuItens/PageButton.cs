using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageButton : MonoBehaviour, IButton {

	LevelButtonManager lb_manager;

	public AudioClip backSFX;
	public int direction;
	Transform worldSelectionScreen;

	void Awake () {
		lb_manager = FindObjectOfType<LevelButtonManager> ();
		worldSelectionScreen = GameObject.Find ("WorldSelection").transform;
	}

	public void OnButtonInteraction () {
		if (GameManager.Instance.c_world.CurrentPage == 0 && direction < 0) {
			MusicManager.Instance.PlaySFX (backSFX);
			CameraSmooth.Instance.Target = worldSelectionScreen;
		} else if (GameManager.Instance.c_world.UpdatePage (direction)) {
			lb_manager.UpdateButtons (GameManager.Instance.c_world.CurrentPage);
		}
	}
}
