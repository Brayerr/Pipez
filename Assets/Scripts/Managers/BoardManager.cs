using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BoardManager : MonoBehaviour
{
    public static Action<Vector2> OnRestrict;
    public static Action<Vector2> OnCancelRestrict;

    public static List<InventorySlot> gameBoard = new List<InventorySlot>();
    [SerializeField] List<InventorySlot> serializedGameBoard = new List<InventorySlot>();
    [SerializeField] List<Pipe> path = new List<Pipe>();

    [SerializeField] Pipe serializedDefaultPipe;
    public static Pipe defaultPipe;

    InventorySlot secondSlot;

    [SerializeField] Vector2 boardSize;

    private bool hasNextTarget = true;
    
    private void Start()
    {
        defaultPipe = serializedDefaultPipe;
        InitializeBoard();
        Pipe.OnPipeTransformChanged += CreatePath;
        PipeController.OnDraggingPipe += CheckColorRestriction;
        PipeController.OnDraggingPipeEnd += ResetColorRestrictions;
    }

    void InitializeBoard()
    {
        Vector2 index = new Vector2(1, 1);
        int iterator = 0;

        foreach (var item in serializedGameBoard)
        {
            gameBoard.Add(item);
        }

        foreach (var item in gameBoard)
        {
            item.SetIndexer(index);
            if (iterator < boardSize.x - 1)
            {
                index = new Vector2(index.x + 1, index.y);
                iterator++;
            }
            else if (iterator >= boardSize.x - 1)
            {
                index = new Vector2(1, index.y + 1);
                iterator = 0;
            }
        }
    }

    public void CheckColorRestriction(string color)
    {
        foreach (var item in gameBoard)
        {
            if (item.state == InventorySlot.State.Occupied && item.pipeObject != null
                && item.pipeObject.state == Pipe.State.inGrid && item.pipeObject.color.ToString() == color)
            {
                foreach (var inventorySlot in gameBoard)
                {
                    if (item.indexer.x == inventorySlot.indexer.x || item.indexer.y == inventorySlot.indexer.y)
                    {
                        if (inventorySlot.pipeObject == null) OnRestrict.Invoke(inventorySlot.indexer);
                    }
                }
            }
        }
    }

    public void ResetColorRestrictions()
    {
        foreach (var item in gameBoard)
        {
            OnCancelRestrict.Invoke(item.indexer);
        }
    }

    void CreatePath()
    {
        bool secondPipeExists = true;
        hasNextTarget = false;
        path.Clear();
        path.Add(defaultPipe);

        Vector2 secondIndex = CalculateSecondSlot(defaultPipe.position, defaultPipe.exitPoints[0], defaultPipe.exitPoints[1]);

        if (secondIndex.x != 0 || secondIndex.y != 0) secondSlot = GetSlot(secondIndex);

        if (secondSlot.pipeObject != null)
        {
            secondPipeExists = true;
            path.Add(secondSlot.pipeObject);
        }

        if (secondPipeExists && secondSlot.pipeObject != null && CheckIfNextPipeConnectedToCurrent(defaultPipe.position, secondIndex)) hasNextTarget = true;

        while (hasNextTarget)
        {
            Vector2 currentIndex = path.Last().position;
            Vector2 nextIndex = CalculateNextSlot(currentIndex);

            if (!CheckNextSlotForObject(nextIndex))
            {
                hasNextTarget = false;
                break;
            }

            else
            {
                if (GetSlot(nextIndex).pipeObject != null && GetSlot(nextIndex).pipeObject.isGoalPiece && CheckIfNextPipeConnectedToCurrent(currentIndex, nextIndex))
                {
                    path.Add(GetSlot(nextIndex).pipeObject);
                    hasNextTarget = false;
                    Debug.Log("level won sequence");
                    break;
                }

                else if (GetSlot(nextIndex).pipeObject != null && CheckIfNextPipeConnectedToCurrent(currentIndex, nextIndex))
                {
                    path.Add(GetSlot(nextIndex).pipeObject);
                    //Debug.Log("added next pipe to path" + path.Last().position);
                }

                else
                {
                    hasNextTarget = false;
                    //Debug.Log("did not find next pipe");
                }
            }
        }
    }

    Vector2 CalculateSecondSlot(Vector2 pipePos, Vector2 exit1, Vector2 exit2)
    {
        Vector2 nextSlot = pipePos + exit1;

        if (nextSlot.x < 1 || nextSlot.y < 1)
        {
            nextSlot = pipePos + exit2;
            return nextSlot;
        }

        else return nextSlot;
    }

    Vector2 CalculateNextSlot(Vector2 currentIndex)
    {
        InventorySlot currentSlot = GetSlot(currentIndex);
        int index = path.Count - 2;


        if (currentSlot.indexer + currentSlot.pipeObject.exitPoints[0] == path.ElementAt(index).position)
        {
            Vector2 nextSlot = currentSlot.indexer + currentSlot.pipeObject.exitPoints[1];
            return nextSlot;
        }

        else if (currentSlot.indexer + currentSlot.pipeObject.exitPoints[1] == path.ElementAt(index).position)
        {
            Vector2 nextSlot = currentSlot.indexer + currentSlot.pipeObject.exitPoints[0];
            return nextSlot;
        }

        else
        {
            Debug.Log("cant find next slot");
            return Vector2.zero;
        }
    }

    bool CheckNextSlotForObject(Vector2 nextIndex)
    {
        InventorySlot nextSlot = GetSlot(nextIndex);
        if (nextSlot.state == InventorySlot.State.Occupied) return true;
        else return false;
    }

    bool CheckIfNextPipeConnectedToCurrent(Vector2 current, Vector2 next)
    {
        InventorySlot nextSlot = GetSlot(next);
        if (nextSlot.pipeObject.exitPoints[0] + nextSlot.indexer == current) return true;
        else if (nextSlot.pipeObject.exitPoints[1] + nextSlot.indexer == current) return true;
        else return false;
    }

    InventorySlot GetSlot(Vector2 index)
    {
        foreach (var item in gameBoard)
        {
            if (item.indexer == index) return item;
        }
        return null;
    }
}