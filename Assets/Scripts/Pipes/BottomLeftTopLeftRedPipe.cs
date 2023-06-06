using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomLeftTopLeftRedPipe : Pipe
{
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
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
                    exitPoints[0] = new Vector2(-1, 1);
                    exitPoints[1] = new Vector2(-1, -1);
                    break;
                }
            case 270:
                {
                    exitPoints[0] = new Vector2(-1, -1);
                    exitPoints[1] = new Vector2(1, -1);
                    break;
                }
            case 180:
                {
                    exitPoints[0] = new Vector2(1, -1);
                    exitPoints[1] = new Vector2(1, 1);
                    break;
                }
            case 90:
                {
                    exitPoints[0] = new Vector2(1, 1);
                    exitPoints[1] = new Vector2(-1, 1);
                    break;
                }
            default:
                break;
        }
    }

}
