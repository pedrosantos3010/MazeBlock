using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(NormalBox))]
public class BoxSFX : MonoBehaviour
{
    [SerializeField] AudioClip movementSFX;
    [SerializeField] AudioClip fallSFX;

    void MoveSFX ()
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlaySFX (movementSFX);
    }

    void FallSFX ()
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlaySFX (fallSFX);
    }

    void OnEnable ()
    {
        NormalBox _box = GetComponent<NormalBox> ();

        _box.OnMove += MoveSFX;
        _box.OnFall += FallSFX;

        if (MusicManager.Instance == null)
            Destroy (this);
    }

    void OnDisable ()
    {
        NormalBox _box = GetComponent<NormalBox> ();

        _box.OnMove -= MoveSFX;
        _box.OnFall -= FallSFX;
    }
}