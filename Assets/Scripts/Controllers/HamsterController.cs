using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HamsterController : MonoBehaviour
{
    [SerializeField] BoardManager boardManager;
    [SerializeField] Transform hamsterTransform;
    [SerializeField] Image hamsterImage;

    [SerializeField] float tweenDuration = 1f;
    State state;
    Vector2 currentDirection;
    Vector3 lastWayPoint;

    enum State
    {
        Idle,
        Walk,
        Climb,
        Fall,
        Land,
        WalkTo,
        ToWalk
    }


    void Start()
    {
        hamsterTransform = transform;
        hamsterImage = GetComponent<Image>();
        BoardManager.OnPathComplete += CreateSequence;
        lastWayPoint = transform.position;
    }

    void CreateSequence()
    {
        Sequence sequence = DOTween.Sequence();
        foreach (var item in boardManager.path)
        {
            Vector2 direction = GetNormalizedRoundedDirection(lastWayPoint, item.wayPoint.position);
            float r = GetZAxisRotation(direction);

            sequence.Append(hamsterTransform.DORotate(new Vector3(0, 0, r), .2f));
            sequence.Append(hamsterTransform.DOMove(item.wayPoint.position, tweenDuration));
            
            lastWayPoint = item.wayPoint.position;
        }
        Debug.Log("added waypoint to sequence");
        MoveThroughPipes(sequence);
    }

    void MoveThroughPipes(Sequence seq)
    {
        hamsterImage.color = new Color(hamsterImage.color.r, hamsterImage.color.g, hamsterImage.color.b, .5f);
        seq.Play();

        Debug.Log("playing sequence");
    }

    Vector2 GetNormalizedRoundedDirection(Vector2 startPos, Vector2 endPos)
    {
        Vector2 holder = (endPos - startPos).normalized;
        holder = new Vector2(Mathf.Round(holder.x), Mathf.Round(holder.y));
        return holder;
    }

    float GetZAxisRotation(Vector2 direction)
    {
        switch (direction)
        {
            case Vector2 v when v == new Vector2(1, 1):
                {
                    return 45;
                }

            case Vector2 v when v == new Vector2(1, -1):
                {
                    return -45;
                }

            case Vector2 v when v == new Vector2(-1, -1):
                {
                    return 45;
                }

            case Vector2 v when v == new Vector2(-1, 1):
                {
                    return -45;
                }
            default:
                return 0;
        }
    }

    void SetState(State state)
    {
        switch (state)
        {
            case State.Idle:
                {
                    break;
                }

            case State.Walk:
                {
                    break;
                }

            case State.Climb:
                {
                    break;
                }

            case State.Fall:
                {
                    break;
                }

            case State.Land:
                {
                    break;
                }

            case State.WalkTo:
                {
                    break;
                }

            case State.ToWalk:
                {
                    break;
                }

            default:
                break;
        }
    }
}
