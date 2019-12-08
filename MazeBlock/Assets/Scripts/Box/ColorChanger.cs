using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{

	public Color actionColor;

	void OnEnable ()
	{
		NormalBox _nb = GetComponent<NormalBox> ();
		if (_nb != null)
			_nb.OnFall += OnChange;
	}

	void OnDisable ()
	{
		NormalBox _nb = GetComponent<NormalBox> ();
		if (_nb != null)
			_nb.OnFall -= OnChange;
	}

	void OnChange ()
	{
		StartCoroutine (ChangeColor ());
	}

	IEnumerator ChangeColor ()
	{
		Renderer _renderer = GetComponent<Renderer> ();

		Color startColor = _renderer.material.color;
		float startTime = Time.time;
		float totalTime = TimeManager.LerpSpeed;
		float percent = 0;

		while (percent < 1) {

			float currentTime = Time.time - totalTime;
			percent = currentTime / totalTime;

			_renderer.material.color = Color.Lerp (startColor, actionColor, percent);

			yield return null;
		}
	}
}
