using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        BoardManager.OnPathComplete += LevelWon;
    }

    void LevelWon()
    {
        Debug.Log("level won sequence");
    }
}
