using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class HamsterController : MonoBehaviour
{
    public static event Action OnSequenceEnd;

    [SerializeField] BoardManager boardManager;
    [SerializeField] Transform hamsterTransform;
    [SerializeField] Image hamsterImage;
    [SerializeField] Animator anim;

    [SerializeField] State currentState = State.Idle;
    [SerializeField] State previousState = State.Idle;
    [SerializeField] Vector2 firstWayPoint;
    [SerializeField] Vector2 lastWayPoint;
    [SerializeField] Transform endPosition;
    [SerializeField] Vector2 direction = new Vector2(1, 0);

    float walkTweenDuration = 1.5f;
    float climbTweenDuration = 2;

    bool isFlipped;



    enum State
    {
        Idle,
        IdleToWalk,
        Walk,
        WalkFlipped,
        WalkTwice,
        Climb,
        Fall,
        Land,
        WalkToClimb,
        WalkToClimbFlipped,
        ClimbToWalk,
        Dance
    }

    private void OnEnable()
    {
        hamsterTransform = transform;
        hamsterImage = GetComponent<Image>();
        anim = GetComponent<Animator>();
        BoardManager.OnPathComplete += CreateSequenceTest;
        GameManager.OnChangedScene += KillAllTweens;
    }

    private void OnDestroy()
    {
        BoardManager.OnPathComplete -= CreateSequenceTest;
        GameManager.OnChangedScene -= KillAllTweens;
    }

    void CreateSequenceTest()
    {
        Sequence sequence = DOTween.Sequence();
        float rotation;
        Tween movementTween;
        Tween rotationTween;
        Tween transitionTween;
        int durationMultiplier = 1;
        sequence.Append(hamsterImage.DOFade(.7f, .2f));
        sequence.AppendCallback(() =>
        {
            SetState(State.IdleToWalk);
        });
        sequence.AppendCallback(() =>
        {
            SetState(State.Walk);
        });

        foreach (var item in boardManager.path)
        {
            direction = GetNormalizedRoundedDirection(lastWayPoint, item.position);
            rotation = GetZAxisRotation(direction);

            if (item.isStraight)
            {
                State s = GetCurrentState(direction);
                sequence.AppendCallback(() =>
                {
                    SetState(s);
                });



                durationMultiplier++;
                continue;
            }

            else
            {
                previousState = currentState;
                currentState = (GetCurrentState(direction));

                //from falling to walking
                if (previousState == State.Fall && currentState == State.Walk)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, .2f);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    transitionTween.OnPlay(() =>
                    {
                        SetState(State.Land);
                    });

                    if (!isFlipped)
                    {
                        movementTween.OnPlay(() =>
                        {
                            SetState(State.Walk);
                        });
                    }
                    else
                    {
                        movementTween.OnPlay(() =>
                        {
                            SetState(State.WalkFlipped);
                        });
                    }

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //from walking to climbing
                else if (previousState == State.Walk && currentState == State.Climb)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .01f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, .2f);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    if (!isFlipped)
                    {
                        transitionTween.OnPlay(() =>
                        {
                            SetState(State.WalkToClimb);
                        });
                    }

                    else
                    {
                        transitionTween.OnPlay(() =>
                        {
                            SetState(State.WalkToClimbFlipped);
                        });
                    }

                    movementTween.OnPlay(() =>
                    {
                        SetState(State.Climb);
                    });

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //from idle to walking
                else if (previousState == State.Idle && currentState == State.Walk)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, .2f);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    transitionTween.OnPlay(() =>
                    {
                        SetState(State.IdleToWalk);
                    });

                    if (!isFlipped)
                    {
                        movementTween.OnPlay(() =>
                        {
                            SetState(State.Walk);
                        });
                    }
                    else
                    {
                        movementTween.OnPlay(() =>
                        {
                            SetState(State.WalkFlipped);
                        });
                    }

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //from walking to falling
                else if (previousState == State.Walk && currentState == State.Fall)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, .2f);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    if (!isFlipped)
                    {
                        transitionTween.OnPlay(() =>
                        {
                            SetState(State.WalkToClimb);
                        });
                    }
                    else
                    {
                        transitionTween.OnPlay(() =>
                        {
                            SetState(State.WalkToClimbFlipped);
                        });
                    }

                    movementTween.OnPlay(() =>
                    {
                        SetState(State.Fall);
                    });

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //from climbing to walking
                else if (previousState == State.Climb && currentState == State.Walk)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, .2f);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    transitionTween.OnPlay(() =>
                    {
                        SetState(State.ClimbToWalk);
                    });

                    if (!isFlipped)
                    {
                        movementTween.OnPlay(() =>
                        {
                            SetState(State.Walk);
                        });
                    }
                    else
                    {
                        movementTween.OnPlay(() =>
                        {
                            SetState(State.WalkFlipped);
                        });
                    }

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //only walking
                else if (currentState == State.Walk)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .1f);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    if (!isFlipped)
                    {
                        movementTween.OnPlay(() =>
                        {
                            SetState(State.Walk);
                        });
                        sequence.Append(rotationTween);
                        sequence.Append(movementTween);
                    }
                    else
                    {
                        movementTween.OnPlay(() =>
                        {
                            SetState(State.WalkFlipped);
                        });
                        sequence.Append(rotationTween);
                        sequence.Join(movementTween);
                    }
                    durationMultiplier = 1;
                }

                //default, this will be changed
                else
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, 0), .2f);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, climbTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                lastWayPoint = item.parent.indexer;
            }
        }
        Debug.Log("finished constructing sequence");
        MoveThroughPipes(sequence);
    }

    void MoveThroughPipes(Sequence seq)
    {
        seq.Play();
        seq.OnComplete(() =>
        {
            SetState(State.Walk);
            hamsterTransform.DORotate(new Vector3(0, 0, 0), .2f).OnComplete(() =>
            {
                hamsterTransform.DOMove(endPosition.position, climbTweenDuration).OnComplete(() =>
                {
                    hamsterImage.color = new Color(hamsterImage.color.r, hamsterImage.color.g, hamsterImage.color.b, 1);
                    hamsterTransform.DOScaleZ(.5f, .2f).OnPlay(() =>
                    {
                        SetState(State.WalkToClimb);
                    }).OnComplete(() =>
                    {
                        hamsterTransform.DOScaleZ(.5f, 2).OnPlay(() =>
                        {
                            SetState(State.Dance);
                        }).OnComplete(() =>
                        {
                            OnSequenceEnd.Invoke();
                        });
                    });
                });
            });
        });
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
                    isFlipped = false;
                    return -45;
                }

            case Vector2 v when v == new Vector2(1, -1):
                {
                    isFlipped = false;
                    return 45;
                }

            case Vector2 v when v == new Vector2(-1, -1):
                {
                    isFlipped = true;
                    return -45;
                }

            case Vector2 v when v == new Vector2(-1, 1):
                {
                    isFlipped = true;
                    return 45;
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
            case Vector2 v when v == new Vector2(0, -1):
                {

                    return State.Climb;
                }

            case Vector2 v when v == new Vector2(0, 1):
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
                    previousState = currentState;
                    currentState = state;
                    anim.SetTrigger("Idle");
                    Debug.Log("idle");
                    break;
                }

            case State.IdleToWalk:
                {
                    previousState = currentState;
                    currentState = state;
                    anim.SetTrigger("IdleToWalkTrigger");
                    Debug.Log("idletowalk");
                    break;
                }

            case State.Walk:
                {
                    previousState = currentState;
                    currentState = state;
                    anim.SetTrigger("WalkTrigger");
                    Debug.Log("walk");
                    break;
                }

            case State.WalkFlipped:
                {
                    previousState = currentState;
                    currentState = state;
                    anim.SetTrigger("WalkFlipped");
                    Debug.Log("walkflipped");
                    break;
                }

            case State.Climb:
                {
                    previousState = currentState;
                    currentState = state;
                    anim.SetTrigger("ClimbTrigger");
                    Debug.Log("climb");
                    break;
                }

            case State.Fall:
                {
                    previousState = currentState;
                    currentState = state;
                    anim.SetTrigger("FallTrigger");
                    Debug.Log("fall");
                    break;
                }

            case State.WalkTwice:
                {
                    previousState = currentState;
                    currentState = state;
                    anim.SetTrigger("WalkTwiceTrigger");
                    Debug.Log("walk twice");
                    break;
                }

            case State.WalkToClimb:
                {
                    previousState = currentState;
                    currentState = state;
                    anim.SetTrigger("WalkToClimb");
                    Debug.Log("walktoclimb");
                    break;
                }

            case State.WalkToClimbFlipped:
                {
                    previousState = currentState;
                    currentState = state;
                    anim.SetTrigger("WalkToClimbFlipped");
                    Debug.Log("walktoclimbflipped");
                    break;
                }

            case State.ClimbToWalk:
                {
                    previousState = currentState;
                    currentState = state;
                    anim.SetTrigger("ClimbToWalk");
                    Debug.Log("climbtowalk");
                    break;
                }
            case State.Land:
                {
                    previousState = currentState;
                    currentState = state;
                    anim.SetTrigger("Land");
                    Debug.Log("land");
                    break;
                }

            case State.Dance:
                {
                    previousState = currentState;
                    currentState = state;
                    anim.SetTrigger("Dance");
                    Debug.Log("dance");
                    break;
                }

            default:
                break;
        }
    }

    void KillAllTweens()
    {
        DOTween.KillAll();
    }
}
