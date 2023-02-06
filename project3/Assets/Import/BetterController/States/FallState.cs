using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FallState : APlayerState
{
    Vector3 moveDir;
    int t, jt;

    public FallState(PlayerDataSO data, ColliderInfo info, CapsuleCollider collider, Rigidbody rigidbody, Action<States> changeStateFunc) : base(data, info, collider, rigidbody, changeStateFunc) { }

    public override void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            data.input = context.ReadValue<Vector2>();
        }

        else if (context.canceled)
        {
            data.input = Vector2.zero;
        }
    }

    public override void Jump(InputAction.CallbackContext context)
    {
        jt = t;
    }

    public override void StateEnd()
    {
        moveDir.y = 0f;

        data.BoolJump(jt + 10 > t);

        if (!data.IsJumpQueued())
        {
            rigidbody.AddForce(2f * data.GetAccelerationCoeff() * moveDir, ForceMode.Acceleration);
        }
    }

    public override void StateStart()
    {
        moveDir = new Vector3();

        t = 0;

        jt = -999;

        data.BoolJump(false);

        Debug.Log("fall");
    }

    public override void StateUpdate()
    {
        if (data.CheckGround(transform.position))
        {
            changeStateFunc((data.input == Vector2.zero) ? States.Idle : States.Move);
        }

        moveDir.x = data.input.x;
        moveDir.y = 0f;
        moveDir.z = data.input.y;
        
        moveDir = transform.TransformDirection(moveDir);
    }

    public override void StateFixedUpdate()
    {
        t++;

        if (!data.IsSpeedCapped(rigidbody.velocity.x, rigidbody.velocity.z))
        {
            rigidbody.AddForce(data.GetAccelerationCoeff() * moveDir, ForceMode.Acceleration);
        }

        rigidbody.AddForce(data.FALLSPEED_BOOST * 10f * Vector3.down, ForceMode.Acceleration);
    }
}
