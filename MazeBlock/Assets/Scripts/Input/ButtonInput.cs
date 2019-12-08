using System.Collections;
using UnityEngine;

public class ButtonInput : MonoBehaviour
{
    public LayerMask ButtonLayer;
    Camera _camera;

    // Use this for initialization
    void Start ()
    {
        _camera = GameObject.FindGameObjectWithTag ("ButtonCamera").GetComponent<Camera> ();
    }

    // Update is called once per frame
    void Update ()
    {
        MouseInput ();
        TouchInput ();
    }

    void MouseInput ()
    {
        if (Input.GetMouseButtonDown (0)) {
            Ray mouseRay = _camera.ScreenPointToRay (Input.mousePosition);
            UseButton (mouseRay);
		}
    }

    void TouchInput ()
    {
        if (Input.touchCount == 0)
            return;

        if (Input.GetTouch (0).phase == TouchPhase.Began) {
            Ray touchRay = _camera.ScreenPointToRay (Input.GetTouch (0).position);
            UseButton (touchRay);
		}
    }

    void UseButton (Ray r_screenToInput)
    {
        RaycastHit hit;
        float rayDistance = 15.0f;

        if (Physics.Raycast (r_screenToInput, out hit, rayDistance, ButtonLayer)) {
            IButton _button = hit.transform.GetComponent<IButton> ();

            if (_button != null)
                _button.OnButtonInteraction ();
		}

    }
}
