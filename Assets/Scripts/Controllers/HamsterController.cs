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
    List<State> states = new List<State>();

    float walkTweenDuration = 1;
    float climbTweenDuration = 2;
    float fallTweenDuration = .8f;
    float transitionTweenDuration = .1f;

    bool isFlipped;

    int iterator = 0;

    enum State
    {
        Idle,
        IdleToWalk,
        Walk,
        WalkFlipped,
        //WalkTwice,
        WalkToWalkFlipped,
        WalkFlippedToWalk,
        Climb,
        Fall,
        Land,
        WalkToClimb,
        WalkToClimbFlipped,
        ClimbToWalk,
        ClimbToWalkFlipped,
        Dance
    }

    private void Start()
    {
        hamsterTransform = transform;
        hamsterImage = GetComponent<Image>();
        anim = GetComponent<Animator>();
        currentState = State.Idle;
        previousState = State.Idle;
    }

    private void OnEnable()
    {
        BoardManager.OnPathComplete += CreateSequence;
        GameManager.OnChangedScene += KillAllTweens;
    }

    private void OnDestroy()
    {
        BoardManager.OnPathComplete -= CreateSequence;
        GameManager.OnChangedScene -= KillAllTweens;
    }

    void CreateSequence()
    {
        Sequence sequence = DOTween.Sequence();
        float rotation;
        Tween movementTween;
        Tween rotationTween;
        Tween transitionTween;
        Tween transitionTween2;
        int durationMultiplier = 1;

        sequence.Append(hamsterImage.DOFade(.7f, .2f));

        states.Add(State.IdleToWalk);
        sequence.AppendCallback(() =>
        {
            SetState(states[iterator]);
            iterator++;
        });

        states.Add(State.Walk);
        sequence.AppendCallback(() =>
        {
            SetState(states[iterator]);
            iterator++;
        });

        foreach (var item in boardManager.path)
        {
            direction = GetNormalizedRoundedDirection(lastWayPoint, item.position);
            rotation = GetZAxisRotation(direction);
            previousState = currentState;
            currentState = GetNextState(direction);

            if (item.isStraight)
            {
                //default if no need for transition
                if (previousState == currentState)
                {
                    states.Add(currentState);
                    sequence.AppendCallback(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });
                    lastWayPoint = item.parent.indexer;
                    durationMultiplier++;
                    continue;
                }

                //idle to walk
                else if (previousState == State.Idle && currentState == State.Walk)
                {
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    states.Add(State.IdleToWalk);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });
                    sequence.Append(transitionTween);
                    lastWayPoint = item.parent.indexer;
                    durationMultiplier++;
                    continue;
                }

                //walk to walk flipped
                else if (previousState == State.Walk && currentState == State.WalkFlipped)
                {
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    states.Add(State.WalkToWalkFlipped);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });
                    sequence.Append(transitionTween);
                    lastWayPoint = item.parent.indexer;
                    durationMultiplier++;
                    continue;
                }

                //walk to climb
                else if (previousState == State.Walk && currentState == State.Climb)
                {
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    states.Add(State.WalkToClimb);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });
                    sequence.Append(transitionTween);
                    lastWayPoint = item.parent.indexer;
                    durationMultiplier++;
                    continue;
                }

                //walk to fall
                else if (previousState == State.Walk && currentState == State.Fall)
                {
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    states.Add(State.WalkToClimb);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });
                    sequence.Append(transitionTween);
                    lastWayPoint = item.parent.indexer;
                    durationMultiplier++;
                    continue;
                }

                //walk flipped to walk
                else if (previousState == State.WalkFlipped && currentState == State.Walk)
                {
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    states.Add(State.WalkFlippedToWalk);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });
                    sequence.Append(transitionTween);
                    lastWayPoint = item.parent.indexer;
                    durationMultiplier++;
                    continue;
                }

                //walk flipped to climb
                else if (previousState == State.WalkFlipped && currentState == State.Climb)
                {
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    states.Add(State.WalkToClimbFlipped);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });
                    sequence.Append(transitionTween);
                    lastWayPoint = item.parent.indexer;
                    durationMultiplier++;
                    continue;
                }

                //walk flipped to fall
                else if (previousState == State.WalkFlipped && currentState == State.Fall)
                {
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    states.Add(State.WalkToClimbFlipped);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });
                    sequence.Append(transitionTween);
                    lastWayPoint = item.parent.indexer;
                    durationMultiplier++;
                    continue;
                }

                //climb to walk
                else if (previousState == State.Climb && currentState == State.Walk)
                {
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    states.Add(State.ClimbToWalk);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });
                    sequence.Append(transitionTween);
                    lastWayPoint = item.parent.indexer;
                    durationMultiplier++;
                    continue;
                }

                //climb to walk flipped
                else if (previousState == State.Climb && currentState == State.WalkFlipped)
                {
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    states.Add(State.ClimbToWalkFlipped);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });
                    sequence.Append(transitionTween);
                    lastWayPoint = item.parent.indexer;
                    durationMultiplier++;
                    continue;
                }

                //fall to walk
                else if (previousState == State.Fall && currentState == State.Walk)
                {
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    states.Add(State.ClimbToWalk);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });
                    sequence.Append(transitionTween);
                    lastWayPoint = item.parent.indexer;
                    durationMultiplier++;
                    continue;
                }

                //fall to walk flipped
                else if (previousState == State.Fall && currentState == State.WalkFlipped)
                {
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    states.Add(State.ClimbToWalkFlipped);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });
                    sequence.Append(transitionTween);
                    lastWayPoint = item.parent.indexer;
                    durationMultiplier++;
                    continue;
                }
            }

            else
            {

                //default - if previous state equals current state - no transition
                if (previousState == currentState)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    states.Add(currentState);
                    movementTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //idle to walk
                else if (previousState == State.Idle && currentState == State.Walk)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    states.Add(State.IdleToWalk);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    states.Add(State.Walk);
                    movementTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //walk to walk flipped
                else if (previousState == State.Walk && currentState == State.WalkFlipped)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    states.Add(State.WalkToWalkFlipped);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    states.Add(State.WalkFlipped);
                    movementTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //walk to climb
                else if (previousState == State.Walk && currentState == State.Climb)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, climbTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    states.Add(State.WalkToClimb);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    states.Add(State.Climb);
                    movementTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //walk to fall
                else if (previousState == State.Walk && currentState == State.Fall)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, fallTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    states.Add(State.WalkToClimb);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    states.Add(State.Fall);
                    movementTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //walk flipped to walk
                else if (previousState == State.WalkFlipped && currentState == State.Walk)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    states.Add(State.WalkFlippedToWalk);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    states.Add(State.Walk);
                    movementTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //walk flipped to climb
                else if (previousState == State.WalkFlipped && currentState == State.Climb)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, climbTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    states.Add(State.WalkToClimbFlipped);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    states.Add(State.Climb);
                    movementTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //walk flipped to fall
                else if (previousState == State.WalkFlipped && currentState == State.Fall)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, fallTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    states.Add(State.WalkToClimbFlipped);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    states.Add(State.Fall);
                    movementTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //climb to walk
                else if (previousState == State.Climb && currentState == State.Walk)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    states.Add(State.ClimbToWalk);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    states.Add(State.Walk);
                    movementTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //climb to walk flipped
                else if (previousState == State.Climb && currentState == State.WalkFlipped)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    states.Add(State.ClimbToWalkFlipped);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    states.Add(State.WalkFlipped);
                    movementTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //fall to walk
                else if (previousState == State.Fall && currentState == State.Walk)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    transitionTween2 = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    states.Add(State.Land);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    states.Add(State.ClimbToWalk);
                    transitionTween2.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    states.Add(State.Walk);
                    movementTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    sequence.Append(transitionTween);
                    sequence.Append(transitionTween2);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //fall to walk flipped
                else if (previousState == State.Fall && currentState == State.WalkFlipped)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    transitionTween2 = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    states.Add(State.Land);
                    transitionTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    states.Add(State.ClimbToWalkFlipped);
                    transitionTween2.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    states.Add(State.WalkFlipped);
                    movementTween.OnPlay(() =>
                    {
                        SetState(states[iterator]);
                        iterator++;
                    });

                    sequence.Append(transitionTween);
                    sequence.Append(transitionTween2);
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
                State s = GetNextState(direction);
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
                currentState = (GetNextState(direction));

                //from idle to walking
                if (previousState == State.Idle && currentState == State.Walk)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
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

                //from walk to walk flipped
                else if (previousState == State.Walk && currentState == State.WalkFlipped)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

                    if (!isFlipped)
                    {
                        transitionTween.OnPlay(() =>
                        {
                            SetState(State.WalkToWalkFlipped);
                        });
                    }
                    //else
                    //{
                    //    transitionTween.OnPlay(() =>
                    //    {
                    //        SetState(State.WalkToClimbFlipped);
                    //    });
                    //}

                    movementTween.OnPlay(() =>
                    {
                        SetState(State.WalkFlipped);
                    });

                    sequence.Append(transitionTween);
                    sequence.Append(rotationTween);
                    sequence.Append(movementTween);
                    durationMultiplier = 1;
                }

                //from walking to climbing
                else if (previousState == State.Walk && currentState == State.Climb)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .01f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, climbTweenDuration * durationMultiplier).SetEase(Ease.Linear);

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

                //from walking to falling
                else if (previousState == State.Walk && currentState == State.Fall)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
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

                //from walk flipped to walk

                //from walk flipped to climb

                //from walk flipped to fall


                //from climbing to walking
                else if (previousState == State.Climb && currentState == State.Walk)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
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

                //from climbing to walk flipped


                //from falling to walking
                else if (previousState == State.Fall && currentState == State.Walk)
                {
                    rotationTween = hamsterTransform.DORotate(new Vector3(0, 0, rotation), .2f);
                    transitionTween = hamsterTransform.DOScaleZ(.5f, transitionTweenDuration);
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

                //from falling to walk flipped



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
                    movementTween = hamsterTransform.DOMove(item.wayPoint.position, walkTweenDuration * durationMultiplier).SetEase(Ease.Linear);

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
            if (currentState == State.Walk)
            {
                SetState(State.Walk);
                hamsterTransform.DORotate(new Vector3(0, 0, 0), .2f).OnComplete(() =>
                {
                    hamsterTransform.DOMove(endPosition.position, walkTweenDuration).OnComplete(() =>
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
            }

            else if (currentState == State.Climb)
            {
                hamsterTransform.DOScaleZ(.5f, transitionTweenDuration).OnPlay(() =>
                {
                    SetState(State.ClimbToWalk);
                }).OnComplete(() =>
                {
                    SetState(State.Walk);
                    hamsterTransform.DORotate(new Vector3(0, 0, 0), .2f).OnComplete(() =>
                    {
                        hamsterTransform.DOMove(endPosition.position, walkTweenDuration).OnComplete(() =>
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
            }
        });
        Debug.Log("playing sequence");
    }



    Vector2 GetNormalizedRoundedDirection(Vector2 startPos, Vector2 endPos)
    {
        if (startPos == Vector2.zero) return new Vector2(1, 0);

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
                    isFlipped = true;
                    return -45;
                }

            case Vector2 v when v == new Vector2(1, -1):
                {
                    isFlipped = true;
                    return 45;
                }

            case Vector2 v when v == new Vector2(-1, -1):
                {
                    isFlipped = false;
                    return -45;
                }

            case Vector2 v when v == new Vector2(-1, 1):
                {
                    isFlipped = false;
                    return 45;
                }

            case Vector2 v when v == new Vector2(-1, 0):
                {
                    isFlipped = false;
                    return 0;
                }

            default:
                {
                    isFlipped = true;
                    return 0;
                }
        }
    }

    State GetNextState(Vector2 direction)
    {
        switch (direction)
        {
            case Vector2 v when v == new Vector2(1, 0):
                {

                    return State.Walk;
                }

            case Vector2 v when v == new Vector2(-1, 0):
                {
                    if (isFlipped) return State.WalkFlipped;
                    else return State.Walk;

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
                    if (isFlipped) return State.WalkFlipped;
                    else return State.Walk;
                }

            case Vector2 v when v == new Vector2(-1, 1):
                {
                    if (isFlipped) return State.WalkFlipped;
                    else return State.Walk;
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
                    //previousState = currentState;
                    //currentState = state;
                    anim.SetTrigger("Idle");
                    Debug.Log("idle");
                    break;
                }

            case State.IdleToWalk:
                {
                    //previousState = currentState;
                    //currentState = state;
                    anim.SetTrigger("IdleToWalkTrigger");
                    Debug.Log("idletowalk");
                    break;
                }

            case State.Walk:
                {
                    //previousState = currentState;
                    //currentState = state;
                    anim.SetTrigger("WalkTrigger");
                    Debug.Log("walk");
                    break;
                }

            case State.WalkFlipped:
                {
                    //previousState = currentState;
                    //currentState = state;
                    anim.SetTrigger("WalkFlipped");
                    Debug.Log("walkflipped");
                    break;
                }

            case State.WalkToWalkFlipped:
                {
                    //previousState = currentState;
                    //currentState = state;
                    anim.SetTrigger("WalkToWalkFlipped");
                    Debug.Log("walk to walkflipped");
                    break;
                }

            case State.WalkFlippedToWalk:
                {
                    //previousState = currentState;
                    //currentState = state;
                    anim.SetTrigger("WalkFlippedToWalk");
                    Debug.Log("walkflipped to walk");
                    break;
                }

            case State.Climb:
                {
                    //previousState = currentState;
                    //currentState = state;
                    anim.SetTrigger("ClimbTrigger");
                    Debug.Log("climb");
                    break;
                }

            case State.Fall:
                {
                    //previousState = currentState;
                    //currentState = state;
                    anim.SetTrigger("FallTrigger");
                    Debug.Log("fall");
                    break;
                }

            //case State.WalkTwice:
            //    {
            //        //previousState = currentState;
            //        //currentState = state;
            //        anim.SetTrigger("WalkTwiceTrigger");
            //        Debug.Log("walk twice");
            //        break;
            //    }

            case State.WalkToClimb:
                {
                    //previousState = currentState;
                    //currentState = state;
                    anim.SetTrigger("WalkToClimb");
                    Debug.Log("walktoclimb");
                    break;
                }

            case State.WalkToClimbFlipped:
                {
                    //previousState = currentState;
                    //currentState = state;
                    anim.SetTrigger("WalkToClimbFlipped");
                    Debug.Log("walktoclimbflipped");
                    break;
                }

            case State.ClimbToWalk:
                {
                    //previousState = currentState;
                    //currentState = state;
                    anim.SetTrigger("ClimbToWalk");
                    Debug.Log("climbtowalk");
                    break;
                }

            case State.ClimbToWalkFlipped:
                {
                    //previousState = currentState;
                    //currentState = state;
                    anim.SetTrigger("ClimbToWalkFlipped");
                    Debug.Log("climb to walkflipped");
                    break;
                }

            case State.Land:
                {
                    //previousState = currentState;
                    //currentState = state;
                    anim.SetTrigger("Land");
                    Debug.Log("land");
                    break;
                }

            case State.Dance:
                {
                    //previousState = currentState;
                    //currentState = state;
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
