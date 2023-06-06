using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    [SerializeField] public static GameObject[,] grid;
    [SerializeField] private Vector2 gridSize;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] public Transform tileMapTransform;

    private void Start()
    {
        CreateTileBoard();
        RenderTileBoard();
    }

    public void CreateTileBoard()
    {
        grid = new GameObject[(int)gridSize.x, (int)gridSize.y];
        Vector2 pos = new Vector2(0, 1);
        for (int i = 0; i < gridSize.x; i++)
        {
            pos = new Vector2(pos.x + 1, 1);

            for (int j = 0; j < gridSize.y; j++)
            {
                grid[j, i] = Instantiate(tilePrefab, pos, Quaternion.identity, tileMapTransform);

                //grid[j, i].SetGridPosition(pos);                
                pos = new Vector2(pos.x, pos.y + 1);
            }
        }
        Debug.Log("created tile map");
    }

 
    public void RenderTileBoard()
    {
        foreach (var item in grid)
        {
            //Instantiate(tilePrefab, item.gridPosition, Quaternion.identity, tileMapTransform);
        }
        Debug.Log("Renderd tile map");
    }


}
