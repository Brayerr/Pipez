using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [SerializeField] Transform panelTransform;
    public List<GameObject> inventory = new List<GameObject>();
    public List<InventorySlot> slots = new List<InventorySlot>();
    [SerializeField] Vector2 inventorySize;


    private void Start()
    {
        PipeController.OnSendToInventoryRequest += SetPipeToEmptySlot;
        PipeController.SentPipeBackToInventory += ReturnPipeToInventory;
        CreateInventoryItems();
        SetIndexersForSlots();
        SetInventoryPipesParents();
    }

    void ReturnPipeToInventory(Vector2 index, Pipe pipe)
    {
        Debug.Log("entered return method");

        foreach (var item in slots)
        {
            if (item.indexer == index)
            {
                item.state = InventorySlot.State.Occupied;
                item.pipeObject = pipe;
                pipe.SetState(Pipe.State.inInventory);
                Debug.Log("found index");

            }
        }
    }

    public void CreateInventoryItems()
    {
        int iterator = 0;
        Pipe pipe;
        foreach (var item in inventory)
        {
            Instantiate(item, slots.ElementAt(iterator).transform);
            pipe = item.GetComponent<Pipe>();
            pipe.SetState(Pipe.State.inInventory);
            iterator++;
        }
    }

    public void SetInventoryPipesParents()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].pipeObject = inventory[i].GetComponent<Pipe>();
            slots[i].pipeObject.parent = slots[i];
            slots[i].pipeObject.parentAfterDrag = slots[i].transform;
        }
    }

    public void SetIndexersForSlots()
    {
        Vector2 indexer = new Vector2(1, 1);
        int iterator = 0;
        foreach (var item in slots)
        {

            item.SetIndexer(indexer);

            if (iterator < inventorySize.x - 1)
            {
                indexer = new Vector2(indexer.x + 1, indexer.y);
                iterator++;
            }
            else if (iterator >= inventorySize.x - 1)
            {
                indexer = new Vector2(1, indexer.y + 1);
                iterator = 0;
            }
            item.state = InventorySlot.State.Occupied;
        }
    }

    public void SetPipeToEmptySlot(Pipe pipe, InventorySlot slot)
    {
        foreach (var item in slots)
        {
            if (item.state == InventorySlot.State.Empty)
            {
                item.pipeObject = pipe;
                item.state = InventorySlot.State.Occupied;
                pipe.RepositionPipe(item.transform);
                pipe.SetState(Pipe.State.inInventory);
                inventory.Add(pipe.gameObject);
                slot.state = InventorySlot.State.Empty;
                slot.pipeObject = null;
                break;
            }
        }
    }
}
