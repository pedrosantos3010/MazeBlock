using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmooth : MonoBehaviour {

	static CameraSmooth _instance;
	public static CameraSmooth Instance {
		get {
			if (_instance == null)
				_instance = FindObjectOfType<CameraSmooth>() as CameraSmooth;

			return _instance;
		}
	}

	Transform lastTarget;
    Transform target;
	public Transform Target {
		set {
			if (value == target || value == null)
				return;
			lastTarget = target;
			target = value;
			StartCoroutine(MoveToTarget());
		}
	}
	Vector3 offset = new Vector3 (-0.5f,12,0);

	public void SetTargetImmediate(Transform newTarget) {
		if (target != null) {
			lastTarget = target;
			lastTarget.gameObject.SetActive(false);
		}
		target = newTarget;
		transform.position = target.position + offset;
    }

	IEnumerator MoveToTarget() {
		target.gameObject.SetActive(true);
		Vector3 targetPosition = target.position + offset;
		Vector3 startPosition = transform.position;

		float percent = 0;
		float timeStartedLerping = Time.time;
		float smooth = TimeManager.LerpSpeed * 5;

		while (percent < 1) {
			float timeSinceStarted = Time.time - timeStartedLerping;
			percent = timeSinceStarted / smooth;

			transform.position = Vector3.Lerp(startPosition, targetPosition, percent);
			yield return null;
		}

		transform.position = targetPosition;

		if (lastTarget != null)
			lastTarget.gameObject.SetActive(false);
	}
}
