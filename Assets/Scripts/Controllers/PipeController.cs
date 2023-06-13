using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PipeController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public static event Action<string> OnDraggingPipe;
    public static event Action OnDraggingPipeEnd;

    [SerializeField] Pipe pipeToControll;

    private void Start()
    {
        pipeToControll = GetComponent<Pipe>();
    }

    private void Update()
    {
        if (pipeToControll.state == Pipe.State.inDrag && Input.GetMouseButtonUp(1)) pipeToControll.RotatePipe();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && pipeToControll.moveable) pipeToControll.RotatePipe();


        else if (pipeToControll.parent.entity == InventorySlot.Entity.Grid && eventData.button == PointerEventData.InputButton.Left && pipeToControll.moveable) Pipe.OnSendToInventoryRequest.Invoke(pipeToControll);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (pipeToControll.moveable)
        {
            pipeToControll.SetState(Pipe.State.inDrag);
            OnDraggingPipe.Invoke(pipeToControll.color.ToString());
            pipeToControll.image.raycastTarget = false;
            pipeToControll.parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            if (pipeToControll.parent != null)
            {
                Pipe.OnPickedPipe.Invoke(pipeToControll.parent.indexer, pipeToControll.parent.entity);
            }
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
        if (pipeToControll.moveable)
        {
            pipeToControll.image.raycastTarget = true;
            pipeToControll.RepositionPipe(pipeToControll.parentAfterDrag);
            OnDraggingPipeEnd.Invoke();
            if (pipeToControll.parent.entity == InventorySlot.Entity.Grid) pipeToControll.SetState(Pipe.State.inGrid);
            else if (pipeToControll.parent.entity == InventorySlot.Entity.Inventory) pipeToControll.SetState(Pipe.State.inInventory);
            if (pipeToControll.parent.entity == InventorySlot.Entity.Grid) Pipe.OnPipeTransformChanged?.Invoke();
        }
    }
}
