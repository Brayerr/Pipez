using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource[] pickUpSources = new AudioSource[3];
    [SerializeField] AudioSource[] placedSources = new AudioSource[3];
    [SerializeField] AudioSource[] nopeSources = new AudioSource[3];
    [SerializeField] AudioSource rotateSource;
    [SerializeField] AudioSource clickSource;
    [SerializeField] AudioSource clackSource;
    [SerializeField] AudioSource returnSource;
    [SerializeField] AudioSource mainMenuBGMSource;

    int chosenIndex;

    private void OnEnable()
    {
        PipeController.PlacedPipe += PlayPlacedSound;
        Pipe.RotatedPipe += PlayRotateSound;
        PipeController.PickedPipe += PlayPickUpSound;
        PipeController.FailedPlacingPipe += PlayNopeSound;
        UIManager.PlayClickSound += PlayClickSound;
        UIManager.PlayClackSound += PlayClackSound;
        PipeController.ReturnedPipe += PlayReturnSound;
        GameManager.OnLoadedMainMenu += PlayMainMenuBGM;
    }

    private void OnDestroy()
    {
        PipeController.PlacedPipe -= PlayPlacedSound;
        Pipe.RotatedPipe -= PlayRotateSound;
        PipeController.PickedPipe -= PlayPickUpSound;
        PipeController.FailedPlacingPipe -= PlayNopeSound;
        UIManager.PlayClickSound -= PlayClickSound;
        UIManager.PlayClackSound -= PlayClackSound;
        PipeController.ReturnedPipe -= PlayReturnSound;
        GameManager.OnLoadedMainMenu -= PlayMainMenuBGM;

    }

    void PlayMainMenuBGM()
    {
        mainMenuBGMSource.Play();
    }

    void PlayReturnSound()
    {
        returnSource.Play();
    }
    void PlayPlacedSound()
    {
        chosenIndex = Random.Range(0, 3);
        placedSources[chosenIndex].Play();
    }

    void PlayPickUpSound()
    {
        chosenIndex = Random.Range(0, 3);
        pickUpSources[chosenIndex].Play();
    }
    void PlayNopeSound()
    {
        chosenIndex = Random.Range(0, 3);
        nopeSources[chosenIndex].Play();
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
