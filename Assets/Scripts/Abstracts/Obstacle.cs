using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Obstacle : MonoBehaviour
{
    //public static event Action<Vector2[]> OnCalculatedBlockedTiles;

    public InventorySlot parent;

    public Vector2[] blockedTiles;
    public Vector2 position;

    private void Start()
    {
        //BoardManager.OnFinishedInitializing += InitilizingSequence;
    }

    void InitilizingSequence()
    {
        //SetPosition();
        CalculateBlockedTiles();
    }

    public abstract void CalculateBlockedTiles();

    void SetPosition()
    {
        position = parent.indexer;
    }

}
