using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class Pipe : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public static Action<Vector2> OnPickedPipe;
    public static Action OnPipeTransformChanged;

    public State state;
    public PipeColor color;
    [SerializeField] Image image;
    [SerializeField] InventorySlot? parent;
    public Transform parentAfterDrag;
    public RectTransform rectTransform;

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (moveable)
        {
            if (parent != null && parent.entity == InventorySlot.Entity.Grid) OnPickedPipe.Invoke(parent.indexer);
            SetState(State.inDrag);
            GameManager.CheckColorRestriction(color.ToString());
            image.raycastTarget = false;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (moveable) transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (moveable)
        {
            image.raycastTarget = true;
            transform.SetParent(parentAfterDrag);
            parent = parentAfterDrag.GetComponent<InventorySlot>();
            GameManager.ResetColorRestrictions();
            position = parent.indexer;
            if (parent.entity == InventorySlot.Entity.Grid) SetState(State.inGrid);
            else if(parent.entity == InventorySlot.Entity.Inventory) SetState(State.inInventory);
            if (parent.entity == InventorySlot.Entity.Grid) OnPipeTransformChanged?.Invoke();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (moveable) RotatePipe();
    }

    public virtual void RotatePipe()
    {
        transform.Rotate(new Vector3(0, 0, -90));
        if (state == State.inGrid) OnPipeTransformChanged?.Invoke();
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
