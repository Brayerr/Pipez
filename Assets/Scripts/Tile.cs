using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IDropHandler
{
    [SerializeField] private Pipe? pipeObject;
    [SerializeField] Transform tileMapTransform;
    public Vector2 gridPosition { get; protected set; }
    public State state { get; protected set; }

    private void Awake()
    {
        SetParentTransform();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (pipeObject == null)
        {
            pipeObject = eventData.pointerDrag.GetComponent<Pipe>();
            pipeObject.transform.position = gridPosition;
            pipeObject.parentAfterDrag = tileMapTransform;
            SetTileState(State.Occupied);
            Debug.Log("dropped");
        }
    }

    public enum State
    {
        Empty,
        Occupied
    }

    public void SetGridPosition(Vector2 pos)
    {
        gridPosition = pos;
    }

    public void SetPipeObject(Pipe pipe)
    {
        pipeObject = pipe;
    }

    public void SetTileState(State state)
    {
        this.state = state;
    }

    public void SetParentTransform()
    {
        tileMapTransform = GetComponentInParent <Transform>();
    }
}
