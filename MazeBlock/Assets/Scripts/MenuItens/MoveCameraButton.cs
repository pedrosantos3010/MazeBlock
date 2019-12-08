using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraButton : MonoBehaviour, IButton {

	public Transform newTarget;
	public AudioClip m_sound;

	// Update is called once per frame
	public void OnButtonInteraction () {

		if (newTarget == null) {
			Application.Quit ();
			return;
		}

		StartCoroutine (UsedAnim());

		print ("Changing to " + newTarget.name);
		if (MusicManager.Instance != null)
			MusicManager.Instance.PlaySFX (m_sound);
		CameraSmooth.Instance.Target = newTarget;
	}

	IEnumerator UsedAnim () {

		Vector3 inicialScale = transform.localScale;
		Vector3 smallScale = transform.localScale + Vector3.one * 0.5f;
		float startTime = Time.time;
		float durationTaken = 0.15f;
		float percent = 0;

		while (percent < 1) {

			percent = (Time.time - startTime) / durationTaken;
			float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;

			transform.localScale = Vector3.Lerp(inicialScale, smallScale, interpolation);

			yield return null;
		}
	}

}
