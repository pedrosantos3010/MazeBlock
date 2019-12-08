using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection
{
	Up,
	Left,
	Down,
	Right,
}

public class SwipeController : MonoBehaviour
{
	public static event System.Action<SwipeDirection> OnSwipe;

	Vector2 startPosition;
	Vector2 direction;
	const float maxDistance = 10;
	bool swiping;

	void Update ()
	{
		SwipeInput ();
		MouseInput ();
	}

	void SwipeInput ()
	{
		if (Input.touchCount == 0)
			return;

		Touch _touch = Input.GetTouch (0);
		if (_touch.phase == TouchPhase.Began) {
			startPosition = _touch.position;
			swiping = true;
		}
		
		if (swiping && _touch.position.sqrMagnitude != 0)
			CheckDirection (_touch.position);

		if (_touch.phase == TouchPhase.Ended && swiping) {
			swiping = false;
		}

	}

	void MouseInput ()
	{
		if (Input.GetMouseButtonDown (0)) {
			startPosition = Input.mousePosition;
			swiping = true;
		}

		if (swiping)
			CheckDirection (Input.mousePosition);

		if (Input.GetMouseButtonUp (0) && swiping) {
			swiping = false;
		}
	}

	void CheckDirection (Vector2 _currentPosition)
	{
		Vector2 distance = _currentPosition - startPosition;
		bool distanceIsEnogh = (Mathf.Abs (distance.x) > maxDistance && Mathf.Abs (distance.y) > maxDistance);

		if (!distanceIsEnogh)
			return;

		SwipeDirection _swipeDirection;
		_swipeDirection = GetDirection (distance);

		if (OnSwipe != null) {
			StopAllCoroutines ();
			StartCoroutine (MoveLoop (_swipeDirection));
		}
		swiping = false;
	}

	SwipeDirection GetDirection (Vector2 distance)
	{

		bool swippingHorizontally = (Mathf.Abs (distance.x) > Mathf.Abs (distance.y));
	
		if (swippingHorizontally)
			return (distance.x > 0) ? SwipeDirection.Right : SwipeDirection.Left;
		else
			return (distance.y > 0) ? SwipeDirection.Up : SwipeDirection.Down;
	}

	IEnumerator MoveLoop (SwipeDirection _swipeDirection)
	{
		bool firstTime = true;
	
		do {
			OnSwipe (_swipeDirection);

			yield return new WaitUntil (() => !TimeManager.timeIsMoving);

			if (firstTime) {
				yield return new WaitForSeconds (TimeManager.LerpSpeed * 2);
				firstTime = false;
			}

			yield return new WaitForSeconds (TimeManager.LerpSpeed * 2);

		} while (Input.GetMouseButton (0) || Input.touchCount != 0);
	}
}
