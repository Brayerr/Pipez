using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class HamsterController : MonoBehaviour
{
    public static event Action OnStartedSequence;

    [SerializeField] BoardManager boardManager;
    [SerializeField] Transform hamsterTransform;
    [SerializeField] Image hamsterImage;
    [SerializeField] Animator anim;

    State state;
    Vector3 lastWayPoint;
    [SerializeField] Transform endPosition;
    State[] sequenceStates;
    int stateIterator = 0;

    float walkTweenDuration = 1.5f;
    float walkTwiceTweenDuration = 4;
    float climbTweenDuration = 2;



    enum State
    {
        Idle,
        Walk,
        WalkTwice,
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
        anim = GetComponent<Animator>();
        BoardManager.OnPathComplete += CreateSequenceTest;
        lastWayPoint = transform.position;
    }

    void CreateSequenceTest()
    {
        Sequence sequence = DOTween.Sequence();
        sequenceStates = new State[boardManager.path.Count];
        Vector2 direction;
        float r;
        State s;
        int iterator = 0;
        Tween tween;
        foreach (var item in boardManager.path)
        {

            direction = GetNormalizedRoundedDirection(lastWayPoint, item.wayPoint.position);
            r = GetZAxisRotation(direction);
            s = GetCurrentState(direction);
            sequenceStates[iterator] = s;

            if (item.isStraight)
            {
                sequence.AppendCallback(() =>
                {
                    if (sequenceStates[stateIterator] == State.Walk)
                    {
                        SetState(State.WalkTwice);
                    }

                    else
                        SetState(sequenceStates[stateIterator]);

                    stateIterator++;
                });

                iterator++;
                continue;
            }



            else
            {
                sequence.AppendCallback(() =>
                {
                    SetState(sequenceStates[stateIterator]);
                    stateIterator++;
                });

                if (sequenceStates[iterator] == State.Walk)
                {
                    sequence.Append(hamsterTransform.DORotate(new Vector3(0, 0, r), .2f));
                    tween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration);
                    sequence.Append(tween);
                }

                else if (sequenceStates[iterator] == State.WalkTwice)
                {
                    sequence.Append(hamsterTransform.DORotate(new Vector3(0, 0, r), .2f));
                    tween = hamsterTransform.DOMove(item.wayPoint.position, walkTwiceTweenDuration);
                    sequence.Append(tween);
                }

                else
                {
                    sequence.Append(hamsterTransform.DORotate(new Vector3(0, 0, 0), .2f));
                    tween = hamsterTransform.DOMove(item.wayPoint.position, climbTweenDuration);
                    sequence.Append(tween);
                }

                lastWayPoint = item.wayPoint.position;
                iterator++;
            }
        }

        Debug.Log("added waypoint to sequence");
        MoveThroughPipes(sequence);
    }


    void MoveThroughPipes(Sequence seq)
    {
        hamsterImage.DOFade(.7f, .1f);
        seq.Play();
        //OnStartedSequence.Invoke();
        seq.OnComplete(() =>
        {
            hamsterTransform.DORotate(new Vector3(0, 0, 0), .2f).OnComplete(() =>
            {
                hamsterTransform.DOMove(endPosition.position, 1);
            });
            
        });
        hamsterImage.DOFade(1, .1f);
        Debug.Log("playing sequence");
    }


    Vector2 GetNormalizedRoundedDirection(Vector2 startPos, Vector2 endPos)
    {
        Vector2 holder = (endPos - startPos);
        holder.Normalize();
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
                {

                    return 0;
                }
        }
    }

    State GetCurrentState(Vector2 direction)
    {
        switch (direction)
        {
            case Vector2 v when v == new Vector2(1, 0):
                {

                    return State.Walk;
                }

            case Vector2 v when v == new Vector2(1, 1):
                {

                    return State.Walk;
                }

            case Vector2 v when v == new Vector2(1, -1):
                {

                    return State.Walk;
                }

            case Vector2 v when v == new Vector2(-1, -1):
                {

                    return State.Walk;
                }

            case Vector2 v when v == new Vector2(-1, 1):
                {

                    return State.Walk;
                }
            case Vector2 v when v == new Vector2(0, 1):
                {

                    return State.Climb;
                }

            case Vector2 v when v == new Vector2(0, -1):
                {

                    return State.Fall;
                }

            default:
                {

                    return State.Idle;
                }
        }
    }

    void SetState(State state)
    {
        switch (state)
        {
            case State.Idle:
                {
                    anim.SetTrigger("IdleToWalkTrigger");
                    Debug.Log("idle");
                    break;
                }

            case State.Walk:
                {
                    anim.SetTrigger("WalkTrigger");
                    Debug.Log("walk");
                    break;
                }

            case State.Climb:
                {
                    anim.SetTrigger("ClimbTrigger");
                    Debug.Log("climb");
                    break;
                }

            case State.Fall:
                {
                    anim.SetTrigger("FallTrigger");
                    Debug.Log("fall");
                    break;
                }

            case State.WalkTwice:
                {
                    anim.SetTrigger("WalkTwiceTrigger");
                    Debug.Log("walk twice");
                    break;
                }

            case State.WalkTo:
                {
                    Debug.Log("walkto");
                    break;
                }

            case State.ToWalk:
                {
                    Debug.Log("towalk");
                    break;
                }

            default:
                break;
        }
    }
}
