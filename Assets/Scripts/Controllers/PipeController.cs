using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PipeController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public static Action<Pipe, InventorySlot> OnSendToInventoryRequest;
    public static event Action<string> OnDraggingPipe;
    public static event Action OnDraggingPipeEnd;
    public static event Action<Vector2, Pipe> SentPipeBackToInventory;
    public static event Action<Vector2, Pipe> SentPipeBackToBoard;

    [SerializeField] Pipe pipeToControll;
    public InventorySlot lastSlot;
    public Vector3 lastPos;

    private void Start()
    {
        pipeToControll = GetComponent<Pipe>();
    }

    private void Update()
    {
        if (pipeToControll.state == Pipe.State.inDrag && Input.GetMouseButtonUp(1)) 
            pipeToControll.RotatePipe();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && pipeToControll.moveable) 
            pipeToControll.RotatePipe();


        else if (pipeToControll.parent.entity == InventorySlot.Entity.Grid && eventData.button == PointerEventData.InputButton.Left 
            && pipeToControll.moveable)
        {
            lastSlot = pipeToControll.parent.GetComponent<InventorySlot>();
            OnSendToInventoryRequest.Invoke(pipeToControll,lastSlot);
            Pipe.OnPipeTransformChanged.Invoke();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (pipeToControll.moveable)
        {
            lastSlot = GetComponentInParent<InventorySlot>();
            lastPos = transform.position;
            pipeToControll.SetState(Pipe.State.inDrag);
            OnDraggingPipe.Invoke(pipeToControll.color.ToString());
            pipeToControll.image.raycastTarget = false;
            pipeToControll.parentAfterDrag = transform.parent;
            transform.SetParent(pipeToControll.parentAfterDrag);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (pipeToControll.moveable)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        pipeToControll.image.raycastTarget = true;
        RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
        if (hit.collider != null)
        {
            InventorySlot slot = hit.collider.gameObject.GetComponent<InventorySlot>();

            if (CheckSlotAvailability(slot))
            {
                PlacePipe(slot);
            }
            else
            {
                ReturnPipe();
            }
        }
        else
        {
            ReturnPipe();
        }
        OnDraggingPipeEnd.Invoke();
    }

    void PlacePipe(InventorySlot slot)
    {
        pipeToControll.parentAfterDrag = slot.transform;
        pipeToControll.RepositionPipe(pipeToControll.parentAfterDrag);
        slot.pipeObject = pipeToControll;
        slot.state = InventorySlot.State.Occupied;

        if (pipeToControll.parent.entity == InventorySlot.Entity.Grid) pipeToControll.SetState(Pipe.State.inGrid);
        else if (pipeToControll.parent.entity == InventorySlot.Entity.Inventory) pipeToControll.SetState(Pipe.State.inInventory);

        if (pipeToControll.parent.entity == InventorySlot.Entity.Grid) Pipe.OnPipeTransformChanged.Invoke();

        if (pipeToControll.parent != null && pipeToControll.parent.obstacleObject == null)
        {
            Pipe.OnPickedPipe.Invoke(lastSlot.indexer, lastSlot.entity);
        }
        pipeToControll.rectTransform.localPosition = new Vector3(pipeToControll.rectTransform.localPosition.x, pipeToControll.rectTransform.localPosition.y, 1);

    }

    void ReturnPipe()
    {
        pipeToControll.RepositionPipe(lastSlot.transform);
        pipeToControll.transform.position = lastPos;
        if (lastSlot.entity == InventorySlot.Entity.Inventory) SentPipeBackToInventory.Invoke(lastSlot.indexer, pipeToControll);
        else if (lastSlot.entity == InventorySlot.Entity.Grid) SentPipeBackToBoard.Invoke(lastSlot.indexer, pipeToControll);
    }

    bool CheckSlotAvailability(InventorySlot slot)
    {
        if (slot.state == InventorySlot.State.Empty && slot.pipeObject == null && slot.obstacleObject == null) return true;
        else if (slot.state == InventorySlot.State.Occupied) return false;
        else return false;
    }
}
