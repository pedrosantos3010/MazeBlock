using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour {

	public GameObject d_music;
	public GameObject d_SFX;

	public Color B_selectedColor;
	public UnityEngine.UI.RawImage pad1_B;
	public UnityEngine.UI.RawImage pad2_B;
	public UnityEngine.UI.RawImage swipe_B;

	// Use this for initialization
	void Awake () {
		if (MusicManager.Instance != null) {
			d_music.SetActive (!MusicManager.Instance.musicEnabled);
			d_SFX.SetActive (!MusicManager.Instance.sfxEnabled);
		}

		UpdateSelectedInputButton ();
	}

	public void Music_config () {
		MusicManager.Instance.ChangeMusicConfig ();
		d_music.SetActive (!MusicManager.Instance.musicEnabled);
	}

	public void SFX_config () {
		d_SFX.SetActive (MusicManager.Instance.sfxEnabled);
		MusicManager.Instance.ChangeSFXConfig ();
	}

	public void ChangeInputMethod (int methodNumber) {
		PlayerPrefs.SetInt ("Input Method", methodNumber);
		UpdateSelectedInputButton ();
	}

	void UpdateSelectedInputButton () {
		switch (PlayerPrefs.GetInt ("Input Method")) {
		case(0):
			pad1_B.color = B_selectedColor;
			pad2_B.color = Color.white;
			swipe_B.color = Color.white;
			break;
		case(1):
			pad1_B.color = Color.white;
			pad2_B.color = B_selectedColor;
			swipe_B.color = Color.white;
			break;
		case(2):
			pad1_B.color = Color.white;
			pad2_B.color = Color.white;
			swipe_B.color = B_selectedColor;
			break;
		}
	}
}
