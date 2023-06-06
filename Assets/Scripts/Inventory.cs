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
        CreateInventoryItems();
        SetIndexersForSlots();
    }

    public void CreateInventoryItems()
    {
        int iterator = 0;
        foreach (var item in inventory)
        {
            Instantiate(item, slots.ElementAt(iterator).transform);
            Pipe pipe = item.GetComponent<Pipe>();
            pipe.SetState(Pipe.State.inInventory);
            iterator++;
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


}
