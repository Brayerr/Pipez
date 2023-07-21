using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource pickUpSource;
    [SerializeField] AudioSource placedSource;
    [SerializeField] AudioSource nopeSource;
    [SerializeField] AudioSource rotateSource;
    [SerializeField] AudioSource clickSource;
    [SerializeField] AudioSource clackSource;


    private void OnEnable()
    {
        PipeController.PlacedPipe += PlayPlacedSound;
        Pipe.RotatedPipe += PlayRotateSound;
        PipeController.PickedPipe += PlayPickUpSound;
        PipeController.ReturnedPipe += PlayNopeSound;
        UIManager.PlayClickSound += PlayClickSound;
        UIManager.PlayClackSound += PlayClackSound;
    }

    private void OnDestroy()
    {
        PipeController.PlacedPipe -= PlayPlacedSound;
        Pipe.RotatedPipe -= PlayRotateSound;
        PipeController.PickedPipe -= PlayPickUpSound;
        PipeController.ReturnedPipe -= PlayNopeSound;
        UIManager.PlayClickSound -= PlayClickSound;
        UIManager.PlayClackSound -= PlayClackSound;
    }

    void PlayPlacedSound()
    {
        placedSource.Play();
    }

    void PlayPickUpSound()
    {
        pickUpSource.Play();
    }
    void PlayNopeSound()
    {
        nopeSource.Play();
    }
    void PlayRotateSound()
    {
        rotateSource.Play();
    }

    void PlayClickSound()
    {
        clickSource.Play();
    }

    void PlayClackSound()
    {
        clackSource.Play();
    }
}
