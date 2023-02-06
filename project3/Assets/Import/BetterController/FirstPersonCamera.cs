using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Camera cam;
    [SerializeField] PlayerDataSO data;
    [SerializeField] float LERP_SPEED = 4f;

    float xRot = 90f;
    float yRot = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GetComponent<Camera>();

        cam.cullingMask = ~(1 << 2);

        Cursor.lockState = CursorLockMode.Confined;
    }

    void LateUpdate()
    {
        if (!IsClose())
        {
            transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime * LERP_SPEED);
        }
    }

    bool IsClose()
    {
        Vector3 dist = transform.position - player.position;

        return dist.sqrMagnitude < 0.02f;
    }

    public void HandleMouse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 vec = context.ReadValue<Vector2>();

            yRot += vec.x * data.SENSITIVITY;
            xRot = Mathf.Clamp(xRot - vec.y * data.SENSITIVITY, 0f, 180f);

            Quaternion qp = Quaternion.Euler(0f, yRot, 0f);
            Quaternion q = Quaternion.Euler(xRot - 90f, 0f, 0f);

            player.localRotation = Quaternion.Lerp(player.localRotation, qp, 0.25f);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, q, 0.25f);


            //Debug.DrawRay(transform.position,transform.forward * 15f, Color.blue, 0.1f);
        }
    }

    public void HandleCamAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {

        }
    }
}
