using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : APlayerState
{
    Vector3 moveDir;
    float multiplier;
    public MoveState(PlayerDataSO data, ColliderInfo info, CapsuleCollider collider, Rigidbody rigidbody, Action<States> changeStateFunc) : base(data, info, collider, rigidbody, changeStateFunc) {}

    public override void Jump(InputAction.CallbackContext context)
    {
        ApplyJump();
    }

    void ApplyJump()
    {
        if (data.CAN_JUMP)
        {
            rigidbody.AddForce(Vector3.up * data.GetJumpForce(), ForceMode.Impulse);

            changeStateFunc.Invoke(States.Rise);
        }
    }

    public override void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            data.input = context.ReadValue<Vector2>();
        }

        else if (context.canceled)
        {
            changeStateFunc.Invoke(States.Idle);
        }
    }

    public override void StateStart()
    {
        moveDir = new Vector3();

        data.SetCurrent(data.standCollider);
        Debug.Log("move");

        if (data.IsJumpQueued())
        {
            rigidbody.AddForce(Vector3.up * -rigidbody.velocity.y, ForceMode.Impulse);

            data.BoolJump(false);

            ApplyJump();
        }
    }

    public override void StateEnd()
    {
        // pass
    }

    public override void StateUpdate()
    {
        if (!data.CheckGround(transform.position))
        {
            changeStateFunc(rigidbody.velocity.y > 0f ? States.Rise : States.Fall);
        }
        
        moveDir.x = data.input.x;
        moveDir.y = 0f;
        moveDir.z = data.input.y;

        moveDir = transform.TransformDirection(moveDir);

        // if we want to turn on a dime, multiple the next force applied by a large amount.
        multiplier = data.GetDirectionCorrectionMultiplier(moveDir, rigidbody.velocity);

        /*
        // if moving in mostly one cardinal direction, become more sensitive to lateral movements.
        if (Mathf.Abs(rigidbody.velocity.x) < Mathf.Abs(rigidbody.velocity.z))
        {
            moveDir.x *= data.TURNING_COEFF;
        }

        else if (Mathf.Abs(rigidbody.velocity.x) > Mathf.Abs(rigidbody.velocity.z))
        {
            moveDir.z *= data.TURNING_COEFF;
        }
        */

        Transform t = data.groundInformation.transform;

        if (t)
        {
            moveDir = t.TransformDirection(moveDir);
        }
    }

    public override void StateFixedUpdate()
    {

        if (!data.IsSpeedCapped(rigidbody.velocity.x, rigidbody.velocity.z))
        {
            rigidbody.AddForce(moveDir * data.GetAccelerationCoeff() * multiplier, ForceMode.Acceleration);
        }
    }

}
