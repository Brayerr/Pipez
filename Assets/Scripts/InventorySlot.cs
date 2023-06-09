using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] public Pipe? pipeObject;
    public Vector2 indexer;
    [SerializeField] public State state;
    [SerializeField] public Entity entity;
    [SerializeField] GameObject x;

    public enum State
    {
        Empty,
        Occupied
    }

    public enum Entity
    {
        Inventory,
        Grid
    }

    private void Start()
    {
        if (entity == Entity.Grid && pipeObject == null) SetEmptyState(indexer);
        GameManager.OnRestrict += SetXTrue;
        GameManager.OnCancelRestrict += SetXFalse;
        Pipe.OnPickedPipe += SetEmptyState;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Pipe pipe = eventData.pointerDrag.GetComponent<Pipe>();
        if (pipe.moveable && pipeObject == null && state == State.Empty)
        {
            pipeObject = pipe;
            pipeObject.parentAfterDrag = transform;
            if (entity == Entity.Inventory) pipeObject.SetState(Pipe.State.inInventory);
            else if (entity == Entity.Grid) pipeObject.SetState(Pipe.State.inGrid);
            state = State.Occupied;
        }
    }

    public void SetIndexer(Vector2 index)
    {
        indexer = index;
    }

    void SetXTrue(Vector2 index)
    {
        if (index == indexer && entity == Entity.Grid && pipeObject == null)
        {
            x.SetActive(true);
            state = State.Occupied;
        }
    }

    void SetXFalse(Vector2 index)
    {
        if (index == indexer && entity == Entity.Grid)
        {
            x.SetActive(false);

            if(pipeObject == null) state = State.Empty;
        }
    }

    void SetEmptyState(Vector2 index)
    {
        if (entity == Entity.Grid && index == indexer && state != State.Empty)
        {
            pipeObject = null;
            state = State.Empty;
            Debug.Log($"grid {indexer} is empty");
        }

        else if(entity == Entity.Inventory && index == indexer && state != State.Empty)
        {
            pipeObject = null;
            state = State.Empty;
            Debug.Log($"inventory {indexer} is empty");
        }
    }
}
