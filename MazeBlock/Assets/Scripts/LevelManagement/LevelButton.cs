using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class LevelButton : MonoBehaviour, IButton {

	public Text t_number;
	private int m_number;

	public Color empetyColor;
	private Color defaultColor;
	private Renderer _renderer;

	public Color[] statusColor;
	public Renderer levelStatus;

	private bool disabled;

	void Awake () {
		_renderer = GetComponent<Renderer> ();
		defaultColor = _renderer.material.color;
	}

	public void UpdateButton (int indexNumber) {
		m_number = indexNumber + 1;
		t_number.text = m_number.ToString();
		_renderer.material.color = defaultColor;
		levelStatus.material.color = (GameManager.Instance.c_world.levelIsComplete[indexNumber] ? statusColor[0] : statusColor[1]);
	}
	 
	public void DeactivateButton () { 
		disabled = true;
		t_number.text = "-";
		_renderer.material.color = empetyColor;
		levelStatus.material.color = statusColor [2];
	}

	public void OnButtonInteraction () {
		if (disabled)
			return;

		GameManager.Instance.LoadSpecificLevel(m_number);
	}



}
