using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RiseState : APlayerState
{
    Vector3 moveDir;

    public RiseState(PlayerDataSO data, ColliderInfo info, CapsuleCollider collider, Rigidbody rigidbody, Action<States> changeStateFunc) : base(data, info, collider, rigidbody, changeStateFunc) { }
   
    public override void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            data.input = context.ReadValue<Vector2>();
            Debug.Log("hey what");
        }

        else if (context.canceled)
        {
            data.input = Vector2.zero;
        }
    }

    public override void StateEnd()
    {
        // pass
    }

    public override void StateStart()
    {
        moveDir = new Vector3();

        Debug.Log("rise");
    }

    public override void StateUpdate()
    {
        moveDir.x = data.input.x;
        moveDir.y = 0f;
        moveDir.z = data.input.y;

        moveDir = transform.TransformDirection(moveDir);

        if (rigidbody.velocity.y < 0f)
        {
            changeStateFunc(States.Fall);
        }

    }

    public override void StateFixedUpdate()
    {
        if (data.input == Vector2.zero)
        {
            Vector3 v = Vector3.zero;
            v.x = rigidbody.velocity.x;
            v.z = rigidbody.velocity.z;

            rigidbody.AddForce(-v, ForceMode.Acceleration);
        }

        if (!data.IsSpeedCapped(rigidbody.velocity.x, rigidbody.velocity.z))
        {
            rigidbody.AddForce(data.GetAccelerationCoeff() * moveDir, ForceMode.Acceleration);
        }

        rigidbody.AddForce(10f * Vector3.down, ForceMode.Acceleration);
    }
}
