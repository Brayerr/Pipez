using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public static event Action GamePaused;
    public static event Action GameUnpaused;
    public static Action PlayClickSound;
    public static Action PlayClackSound;

    [SerializeField] Canvas? boardCanvas;
    [SerializeField] Canvas? inventoryCanvas;
    [SerializeField] Camera? mainCam;
    [SerializeField] Camera? secondCam;

    [SerializeField] Image pauseMenu;
    [SerializeField] Image VictoryMenu;

    bool pauseMenuOpen;

    private void OnEnable()
    {
        BoardManager.OnPathComplete += ChangeCanvasRenderMode;
        HamsterController.OnSequenceEnd += ChangeCanvasBack;
        GameManager.OnClickedEsc += OpenPauseMenu;
        HamsterController.OnSequenceEnd += OpenVictoryMenu;
    }

    private void OnDestroy()
    {
        BoardManager.OnPathComplete -= ChangeCanvasRenderMode;
        HamsterController.OnSequenceEnd -= ChangeCanvasBack;
        GameManager.OnClickedEsc -= OpenPauseMenu;
        HamsterController.OnSequenceEnd -= OpenVictoryMenu;
    }

    void ChangeCanvasRenderMode()
    {
        inventoryCanvas.gameObject.SetActive(false);
        boardCanvas.renderMode = RenderMode.WorldSpace;
    }

    void ChangeCanvasBack()
    {
        inventoryCanvas.gameObject.SetActive(true);
        boardCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    #region PAUSE_MENU
    public void OpenPauseMenu()
    {
        if (!pauseMenuOpen)
        {
            PlayClickSound.Invoke();
            Sequence seq = DOTween.Sequence();
            seq.Append(pauseMenu.DOFade(1, .4f));
            seq.Join(pauseMenu.transform.DOScale(1, .4f));
            seq.OnPlay(() =>
            {
                pauseMenu.gameObject.SetActive(true);
            });
            seq.Play();
            seq.OnComplete(() =>
            {
                GamePaused.Invoke();
            });
            pauseMenuOpen = true;
        }
        else
        {
            PlayClackSound.Invoke();
            Sequence seq = DOTween.Sequence();
            seq.Append(pauseMenu.DOFade(0, .4f));
            seq.Join(pauseMenu.transform.DOScale(0.1f, .4f));
            seq.Play();
            seq.OnComplete(() =>
            {
                pauseMenu.gameObject.SetActive(false);
                GameUnpaused.Invoke();
            });
            pauseMenuOpen = false;
        }
    }
    #endregion

    #region VICTORY_MENU
    public void OpenVictoryMenu()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(VictoryMenu.DOFade(1, .4f));
        seq.Join(VictoryMenu.transform.DOScale(1, .4f));
        seq.OnPlay(() =>
        {
            VictoryMenu.gameObject.SetActive(true);
        });
        seq.Play();
    }

    public void CloseVictoryMenu()
    {
        PlayClickSound.Invoke();
        Sequence seq = DOTween.Sequence();
        seq.Append(VictoryMenu.DOFade(0, .4f));
        seq.Join(VictoryMenu.transform.DOScale(0.1f, .4f));
        seq.Play();
        seq.OnComplete(() =>
        {
            VictoryMenu.gameObject.SetActive(false);
        });
    }
    #endregion


}
