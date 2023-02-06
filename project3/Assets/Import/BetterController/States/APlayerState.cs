using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public abstract class APlayerState
{
    protected Action<States> changeStateFunc;
    protected PlayerDataSO data;
    protected CapsuleCollider collider;
    protected ColliderInfo state_collider_info;
    protected Rigidbody rigidbody;
    protected Transform transform;

    public APlayerState(PlayerDataSO data, ColliderInfo state_collider_info, CapsuleCollider collider, Rigidbody rigidbody, Action<States> changeStateFunc)
    {
        this.data = data;
        this.collider = collider;
        this.state_collider_info = state_collider_info;
        this.rigidbody = rigidbody;
        this.changeStateFunc = changeStateFunc;
        transform = rigidbody.transform;

        if (data.CheckIfEmpty())
        {
            Debug.LogError("SET VALUES OF ACTION FUNCTIONS!");
        }
    }

    protected void SetColliderParameters()
    {
        collider.center = state_collider_info.center;
        collider.radius = state_collider_info.radius;
        collider.height = state_collider_info.height;
        collider.direction = state_collider_info.direction;
    }

    // Handles a Vector2 input for lateral movement.
    public virtual void Move(InputAction.CallbackContext context)
    {
        // pass
    }
    // Handles a button press for jumping.
    public virtual void Jump(InputAction.CallbackContext context)
    {
        // pass
    }
    // Handles a button hold to crouch.
    public virtual void Crouch(InputAction.CallbackContext context)
    {
        // pass
    }
    // Handles a button press to dodge in a direction.
    public virtual void Dodge(InputAction.CallbackContext context)
    {
        // pass
    }
    // Performs a player action.
    public virtual void Action(InputAction.CallbackContext context)
    {
        if (!data.CAN_ACT)
        {
            return;
        }


        if (data.CheckAction(transform.position, transform.forward))
        {
            Collider collider = data.actionInformation.collider;

            if (collider.CompareTag("Throwable"))
            {
                data.FireAction(1);
            }

            else if (collider.CompareTag("Window"))
            {
                data.FireAction(0);
            }
        }

        else
        {
            data.FireAction(2);
        }
    }
    // Performs an action related to a camera.
    // public abstract void CameraAction(InputAction.CallbackContext context);
    // Opens a settings menu.
    // public abstract void OpenMenu(InputAction.CallbackContext context);


    public virtual void StateFixedUpdate()
    {
        // pass
    }
    public abstract void StateUpdate();
    public abstract void StateStart();
    public abstract void StateEnd();
}
