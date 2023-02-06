using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachine : MonoBehaviour
{
    [SerializeField] PlayerDataSO data;
    Dictionary<States, APlayerState> table;

    APlayerState current;
    APlayerState previous;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;

        CapsuleCollider c = GetComponent<CapsuleCollider>();
        Rigidbody r = GetComponent<Rigidbody>();

        data.ResetData(transform.position);

        StateFactory factory = new StateFactory(
            data,
            c,
            new ColliderInfo[] { data.standCollider, data.crouchCollider },
            r,
            ChangeState
            );

        table = new Dictionary<States, APlayerState>() 
        {
            { States.Idle, factory.CreateState(States.Idle) },
            { States.Move, factory.CreateState(States.Move) },
            { States.Rise, factory.CreateState(States.Rise) },
            { States.Fall, factory.CreateState(States.Fall) }
        };

        current = table[States.Idle];
    }

    // ticks every frame.
    private void Update()
    {
        //Debug.Log(Input.GetAxis("Mouse X"));

        current.StateUpdate();

        Debug.DrawRay(transform.position + 0.5f * 2f * Vector3.up, Vector3.down * 2.1f, Color.green, 0.5f);
    }

    // ticks every 0.02 seconds.
    private void FixedUpdate()
    {
        current.StateFixedUpdate();
    }

    public void ChangeState(States state)
    {
        current.StateEnd();

        previous = current;

        current = table[state];

        current.StateStart();
    }

    void IfPerformed(InputAction.CallbackContext c, Action<InputAction.CallbackContext> f)
    {
        if (c.performed)
        {
            f.Invoke(c);
        }
    }

    public void HandleMove(InputAction.CallbackContext context)
    {
        current.Move(context);
    }

    public void HandleJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            current.Jump(context);
        }
    }

    public void HandleCrouch(InputAction.CallbackContext context)
    {
        current.Crouch(context);
    }

    public void HandleDodge(InputAction.CallbackContext context)
    {
        IfPerformed(context, current.Dodge);
    }

    public void HandleAction(InputAction.CallbackContext context)
    {
        IfPerformed(context, current.Action);
    }
}
