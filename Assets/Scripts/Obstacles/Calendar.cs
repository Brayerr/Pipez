using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calendar : Obstacle
{

    public override void CalculateBlockedTiles()
    {
        blockedTiles = new Vector2[4];
        blockedTiles[0] = position;
        blockedTiles[1] = new Vector2(position.x + 1, position.y);
        blockedTiles[2] = new Vector2(position.x, position.y + 1);
        blockedTiles[3] = new Vector2(position.x + 1, position.y + 1);
    }
}
