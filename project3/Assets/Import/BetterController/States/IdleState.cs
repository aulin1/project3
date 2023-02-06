using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : APlayerState
{
    public IdleState(PlayerDataSO data, ColliderInfo info, CapsuleCollider collider, Rigidbody rigidbody, Action<States> changeStateFunc) : base(data, info, collider, rigidbody, changeStateFunc) { }

    public override void Move(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            changeStateFunc(States.Move);
        }
    }

    public override void Jump(InputAction.CallbackContext context)
    {
        if (data.CAN_JUMP)
        {
            rigidbody.AddForce(Vector3.up * data.GetJumpForce(), ForceMode.Impulse);

            changeStateFunc.Invoke(States.Rise);
        }
    }

    public override void StateEnd()
    {
        // pass
    }

    public override void StateStart()
    {
        data.input = Vector2.zero;

        data.BoolJump(false);

        Debug.Log("idle");
    }

    public override void StateUpdate()
    {
        if (!data.CheckGround(transform.position))
        {
            changeStateFunc(States.Fall);
        }

        //Debug.Log(data.CheckGround(transform.position));
    }

    public override void StateFixedUpdate()
    {
        if (rigidbody.velocity.sqrMagnitude > 0.25f)
        {
            rigidbody.AddForce(-rigidbody.velocity * 6f, ForceMode.Acceleration);
        }
    }
}
