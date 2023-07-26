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
    [SerializeField] AudioSource wooSource;
    [SerializeField] AudioSource mainMenuBGMSource;
    [SerializeField] AudioSource levelBGMSource;
    [SerializeField] AudioSource sequenceBGMSource;

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
        HamsterController.OnSequenceStart += PlaySequenceBGM;
        HamsterController.OnDance += PlayWooSource;
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
        HamsterController.OnSequenceStart -= PlaySequenceBGM;
        HamsterController.OnDance -= PlayWooSource;


    }

    public void MuteBGM()
    {
        if (levelBGMSource.volume != 0)
        {
            levelBGMSource.volume = 0;
            mainMenuBGMSource.volume = 0;
            sequenceBGMSource.volume = 0;
        }
        else
        {
            levelBGMSource.volume = 1;
            mainMenuBGMSource.volume = 1;
            sequenceBGMSource.volume = 1;
        }
    }

    public void MuteSFX()
    {
        if (rotateSource.volume != 0)
        {
            rotateSource.volume = 0;
            clickSource.volume = 0;
            clackSource.volume = 0;
            returnSource.volume = 0;
            wooSource.volume = 0;
            pickUpSources[0].volume = 0;
            pickUpSources[1].volume = 0;
            pickUpSources[2].volume = 0;
            placedSources[0].volume = 0;
            placedSources[1].volume = 0;
            placedSources[2].volume = 0;
            nopeSources[0].volume = 0;
            nopeSources[1].volume = 0;
            nopeSources[2].volume = 0;
        }
        else
        {
            rotateSource.volume = 1;
            clickSource.volume = 1;
            clackSource.volume = 1;
            returnSource.volume = 1;
            wooSource.volume = 1;
            pickUpSources[0].volume = 1;
            pickUpSources[1].volume = 1;
            pickUpSources[2].volume = 1;
            placedSources[0].volume = 1;
            placedSources[1].volume = 1;
            placedSources[2].volume = 1;
            nopeSources[0].volume = 1;
            nopeSources[1].volume = 1;
            nopeSources[2].volume = 1;
        }
    }

    void PlayWooSource()
    {
        wooSource.Play();
    }

    void PlaySequenceBGM()
    {
        levelBGMSource.Stop();
        sequenceBGMSource.Play();
    }

    void PlayLevelBGM()
    {
        levelBGMSource.Play();
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
