using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas boardCanvas;
    [SerializeField] Camera mainCam;
    [SerializeField] Camera secondCam;


    private void Start()
    {
        BoardManager.OnPathComplete += ChangeCanvasRenderMode;
    }

    void SwitchCamera()
    {
        mainCam.enabled = false;
        secondCam.enabled = true;
    }

    void ChangeCanvasRenderMode()
    {
        boardCanvas.renderMode = RenderMode.WorldSpace;
    }
}
