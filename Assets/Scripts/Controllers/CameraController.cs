using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform hamsterTransform;

    bool shouldFollow = false;
    private void Start()
    {
        cam = GetComponent<Camera>();
        HamsterController.OnStartedSequence += ZoomIn;
    }

    private void Update()
    {
        if(shouldFollow)
        {
            FollowHamster();
        }
    }

    void FollowHamster()
    {
            cam.transform.position = hamsterTransform.position;
    }

    void ZoomIn()
    {
        cam.orthographicSize = 1.5f;
        shouldFollow = true;
    }
}
