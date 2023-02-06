using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum States
{
    Idle,
    Move,
    Rise,
    Fall,
    CrouchIdle,
    CrouchMove
}

public class StateFactory
{
    PlayerDataSO data;
    CapsuleCollider state_collider;
    ColliderInfo[] infos;
    Rigidbody rigidbody;
    Action<States> changeStateFunc;

    public StateFactory(PlayerDataSO data, CapsuleCollider state_collider, ColliderInfo[] infos, Rigidbody rigidbody, Action<States> changeStateFunc)
    {
        this.data = data;
        this.state_collider = state_collider;
        this.infos = infos;
        this.rigidbody = rigidbody;
        this.changeStateFunc = changeStateFunc;
    }

    public APlayerState CreateState(States state)
    {
        return state switch
        {
            States.Idle => new IdleState(data, infos[0], state_collider, rigidbody, changeStateFunc),
            States.Move => new MoveState(data, infos[0], state_collider, rigidbody, changeStateFunc),
            States.Rise => new RiseState(data, infos[0], state_collider, rigidbody, changeStateFunc),
            States.Fall => new FallState(data, infos[0], state_collider, rigidbody, changeStateFunc),
                                // case States.CrouchIdle:
                                //    return null;
                                // case States.CrouchMove:
                                //    return null;
            _ => throw new NotImplementedException(),
        };
    }
}
