using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour {
	
	public Animator blackScreen;

	static InputManager _intance;
	public static InputManager Instance {
		get {
			return _intance;
		}
	}
		
	public GameObject controllerPad1, controllerPad2;

	private SwipeController _swipeController;
	private ButtonInput _buttonInput;

	public float z_levelOffset;
	int inputMethod; 
	public int InputMethod {
		set {
			inputMethod = value;
			ChangeInputMethod ();
		}
	}

	void Awake () {
		_swipeController = GetComponent<SwipeController> ();
		_buttonInput = GetComponent<ButtonInput> ();

		inputMethod = PlayerPrefs.GetInt ("Input Method");
		ChangeInputMethod ();

	}

	public void ResetGame() {
		if (GameManager.Instance != null)
			StartCoroutine (GameManager.Instance.RestartLevel ());
		StartFadeOut ();
	}

	void StartFadeOut() {
		StartCoroutine(ScreenFadeOut());
	}
	
	IEnumerator ScreenFadeOut() {
		blackScreen.SetTrigger("FadeOut");
		
		yield return new WaitForSeconds(0.1f);
	}

	public void PauseGame () {
		GameManager.Instance.LoadMenu();
		StartCoroutine(ScreenFadeOut());
	}

	//Change between Input Methods
	void ChangeInputMethod() {

		#if UNITY_EDITOR
		if (Application.isEditor && !Application.isPlaying) {
			_swipeController = GetComponent<SwipeController> ();
			_buttonInput = GetComponent<ButtonInput> ();
		}
		#endif

		switch (inputMethod) {
		case(0)://Pad1
			controllerPad1.SetActive (true);
			controllerPad2.SetActive (false);
			_swipeController.enabled = false;
			_buttonInput.enabled = true;
			RemoveZOffset ();
			break;
		case(1)://Pad2
			controllerPad1.SetActive (false);
			controllerPad2.SetActive (true);
			_swipeController.enabled = false;
			_buttonInput.enabled = true;
			RemoveZOffset ();
			break;
		case(2)://Swipe
			controllerPad1.SetActive (false);
			controllerPad2.SetActive (false);
			_swipeController.enabled = true;
			_buttonInput.enabled = false;
			ApplyZOffset ();
			break;
		}
	}

	public void ApplyZOffset () {
		Vector3 c_position = Camera.main.transform.position;
		c_position.z = z_levelOffset;

		Camera.main.transform.position = c_position;
	}

	void RemoveZOffset () {
		Vector3 c_position = Camera.main.transform.position;
		c_position.z = 0;

		Camera.main.transform.position = c_position;
	}

	void OnEnable() {
		_intance = this;
		if (GameManager.Instance != null)
			GameManager.Instance.LoadActions += StartFadeOut;
		else {
			GameObject _gameManager = new GameObject ("Game Manager");
			_gameManager.AddComponent (typeof(GameManager));
		}
	}

	void OnDisable () {
		if (GameManager.Instance != null)
			GameManager.Instance.LoadActions -= StartFadeOut;
	}
}
