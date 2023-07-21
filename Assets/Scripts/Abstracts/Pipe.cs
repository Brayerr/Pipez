using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public abstract class Pipe : MonoBehaviour
{

    public static Action<Vector2, InventorySlot.Entity> OnPickedPipe;
    public static Action OnPipeTransformChanged;
    public static event Action RotatedPipe;

    public State state;
    public PipeColor color;
    [SerializeField] public Image image;
    [SerializeField] public InventorySlot parent;
    public Transform parentAfterDrag;
    public RectTransform rectTransform;
    [SerializeField] public Transform wayPoint;

    [SerializeField] public Vector2 position;
    public bool isConnected = false;
    public bool moveable = true;
    public bool isStraight = false;
    [SerializeField] public bool isGoalPiece = false;

    public Vector2[] exitPoints = new Vector2[2];

    public enum State
    {
        inInventory,
        inGrid,
        inDrag
    }

    public enum PipeColor
    {
        Red,
        Orange,
        Green,
        Blue,
        Purple,
        Yellow,
        Black
    }

    public void RepositionPipe(Transform parentAfterDrag)
    {
        this.parentAfterDrag = parentAfterDrag;
        transform.SetParent(parentAfterDrag);
        parent = parentAfterDrag.GetComponent<InventorySlot>();
        position = parent.indexer;
    }

    public virtual void RotatePipe()
    {
        transform.Rotate(new Vector3(0, 0, -90));
        RotatedPipe.Invoke();
        //transform.DORotate(new Vector3(0, 0, -90), .1f, RotateMode.WorldAxisAdd);
        if (state == State.inGrid || state == State.inDrag) OnPipeTransformChanged.Invoke();
    }

    public void SetState(State state)
    {
        switch (state)
        {
            case State.inInventory:
                {
                    this.state = state;
                    transform.DOScale(.5f, .1f).OnComplete(() =>
                    {
                        transform.DOScale(.4f, .1f).OnComplete(() =>
                        {
                            transform.DOScale(.5f, .1f);
                        });
                    });
                    break;
                }
            case State.inGrid:
                {
                    this.state = state;
                    transform.DOScale(.85f, .1f).OnComplete(() =>
                    {
                        transform.DOScale(.75f, .1f).OnComplete(() =>
                        {
                            transform.DOScale(.85f, .1f);
                        });
                    });
                    break;
                }
            case State.inDrag:
                {
                    this.state = state;
                    transform.DOScale(.85f, .1f);
                    break;
                }
        }
    }

    public abstract void CalculateExitPoints();

}
