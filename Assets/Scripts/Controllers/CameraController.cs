using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{

    [SerializeField] Transform hamsterTransform;
    [SerializeField] Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();

    }

    private void Update()
    {

        FollowHamster();
    }

    void FollowHamster()
    {
        cam.transform.position = new Vector3(hamsterTransform.position.x, hamsterTransform.position.y, cam.transform.position.z);
    }

    void MoveCamera(Vector2 pos)
    {
        transform.DOMove(pos, .5f);
    }
}
