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
        Pipe.OnSendToInventoryRequest += SetPipeToEmptySlot;
        CreateInventoryItems();
        SetIndexersForSlots();
        SetInventoryPipesParents();
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
            //pipe.RepositionPipe(slots.ElementAt(iterator).transform);
            //slots.ElementAt(iterator).pipeObject = pipe;
            //Debug.Log(slots.ElementAt(iterator).indexer);
            iterator++;
        }
    }

    public void SetInventoryPipesParents()
    {
        //int iterator = 0;
        //foreach (var item in slots)
        //{
        //    slots.ElementAt(iterator).pipeObject = inventory.ElementAt(iterator).GetComponent<Pipe>();
        //    slots.ElementAt(iterator).pipeObject.parent = slots.ElementAt(iterator);
        //    slots.ElementAt(iterator).pipeObject.parentAfterDrag = slots.ElementAt(iterator).transform;
        //    iterator++;
        //}

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

    public void SetPipeToEmptySlot(Pipe pipe)
    {
        foreach (var item in slots)
        {
            if (item.state == InventorySlot.State.Empty)
            {
                Debug.Log($"sending pipe back to inventory to slot - {item.indexer}");
                item.pipeObject = pipe;
                item.state = InventorySlot.State.Occupied;
                pipe.RepositionPipe(item.transform);
                pipe.SetState(Pipe.State.inInventory);
                inventory.Add(pipe.gameObject);
                break;
            }
        }
    }
}
