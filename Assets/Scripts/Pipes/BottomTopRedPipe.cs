using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomTopRedPipe : Pipe
{

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        isStraight = true;
        CalculateExitPoints();
    }
    public override void RotatePipe()
    {
        base.RotatePipe();
        CalculateExitPoints();
    }

    public override void CalculateExitPoints()
    {
        switch (rectTransform.localEulerAngles.z)
        {
            case 0:
                {
                    exitPoints[0] = new Vector2(0, 1);
                    exitPoints[1] = new Vector2(0, -1);
                    break;
                }
            case 270:
                {
                    exitPoints[0] = new Vector2(-1, 0);
                    exitPoints[1] = new Vector2(1, 0);
                    break;
                }
            case 180:
                {
                    exitPoints[0] = new Vector2(0, 1);
                    exitPoints[1] = new Vector2(0, -1);
                    break;
                }
            case 90:
                {
                    exitPoints[0] = new Vector2(-1, 0);
                    exitPoints[1] = new Vector2(1, 0);
                    break;
                }
            default:
                break;
        }
    }
}
