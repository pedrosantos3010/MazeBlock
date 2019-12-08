using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class WorldButton : MonoBehaviour, IButton {

	public Transform levelSelection;
	public World m_world;

	public Text t_number;
	public Text t_score;
	public Image i_locked;
	public Renderer _renderer;
	public AudioClip m_sound;

	private LevelButtonManager lb_manager;

	private bool locked;

	public bool winAllLevels;

	void Awake () {
		lb_manager = FindObjectOfType<LevelButtonManager> ();
	}

	public void UpdateButton () {
		if (winAllLevels) {
			m_world.WinAll ();
			winAllLevels = false;
		}

		m_world.Load ();

		if (m_world.levelCount == 0) {
			WorldInContruction ();
			return;
		}
			
		locked = m_world.levelsNeeded > GameManager.Instance.totalCompleteLevels;

		_renderer.material.color = m_world.worldColor;

		if (locked) {
			i_locked.enabled = true;
			t_number.enabled = false;

			t_score.text = GameManager.Instance.totalCompleteLevels.ToString () + "/" + m_world.levelsNeeded.ToString();
		} else {
			i_locked.enabled = false;
			t_number.enabled = true;

			t_number.text = m_world.number.ToString ();
			t_score.text = m_world.completeLevelsCount.ToString () + "/" + m_world.levelCount.ToString ();
		}
	}

	public void WorldInContruction () {
		locked = true;
		_renderer.material.color = m_world.worldColor;

		i_locked.enabled = true;
		t_number.enabled = false;

		t_score.text = "Soon";
	}

	public void OnButtonInteraction () {
		if (locked)
			return;

		MusicManager.Instance.PlaySFX (m_sound);
		CameraSmooth.Instance.Target = levelSelection;
		GameManager.Instance.c_world = m_world;
		GameManager.Instance.c_world.UpdatePage(0);
		lb_manager.UpdateButtons (GameManager.Instance.c_world.CurrentPage);
		lb_manager.UpdateWorldLabel (m_world.number, t_score.text, m_world.worldColor);
	}
}
