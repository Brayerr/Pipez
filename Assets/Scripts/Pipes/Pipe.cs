using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class Pipe : MonoBehaviour
{

    public static Action<Vector2, InventorySlot.Entity> OnPickedPipe;
    public static Action OnPipeTransformChanged;

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
        if (state == State.inGrid) OnPipeTransformChanged.Invoke();
    }

    public void SetState(State state)
    {
        switch (state)
        {
            case State.inInventory:
                {
                    this.state = state;
                    transform.localScale = new Vector3(.5f, .5f, 1);
                    break;
                }
            case State.inGrid:
                {
                    this.state = state;
                    transform.localScale = new Vector3(.85f, .85f, 1);
                    break;
                }
            case State.inDrag:
                {
                    this.state = state;
                    transform.localScale = new Vector3(.85f, .85f, 1);
                    break;
                }
        }
    }

    public abstract void CalculateExitPoints();

}
