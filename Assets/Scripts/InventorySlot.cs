using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : MonoBehaviour
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
        if (indexer != Vector2.zero && entity == Entity.Grid && pipeObject == null) SetEmptyState(indexer, entity);
        BoardManager.OnRestrict += SetXTrue;
        BoardManager.OnCancelRestrict += SetXFalse;
        Pipe.OnPickedPipe += SetEmptyState;
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

    void SetEmptyState(Vector2 index, Entity entity)
    {
        switch(entity)
        {
            case Entity.Grid:
                {
                    if(index == indexer && state != State.Empty && this.entity == Entity.Grid)
                    {
                        pipeObject = null;
                        state = State.Empty;
                        Debug.Log($"grid {indexer} is empty");
                    }
                    break;
                }
            case Entity.Inventory:
                {
                    if(index == indexer && state != State.Empty && this.entity == Entity.Inventory)
                    {
                        pipeObject = null;
                        state = State.Empty;
                        Debug.Log($"inventory {indexer} is empty");
                    }
                    break;
                }
            default:
                break;
        }
    }
}
