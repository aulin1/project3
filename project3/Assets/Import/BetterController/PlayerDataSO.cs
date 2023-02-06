using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    [Header("System Settings")]
    [Tooltip("If you are unsure, probably don't touch these.")]
    [Range(0.001f, 0.05f)]
    [SerializeField] float FIXED_TIMESTEP = 0.02f;
    [SerializeField] int DEL_WARNING_WINDOW = 2, DEL_WARNING_ACTION = 2, DEL_WARNING_THROW = 2;
    [Header("Player Settings")]
    [SerializeField] int MAX_INCLINE_ANGLE = 75;
    [SerializeField] float MAX_MOVE_SPEED = 9f;
    [SerializeField] float MAX_FALL_SPEED = 25f; // unused?
    [SerializeField] float ACCELERATION = 5f;
    [SerializeField] float JUMP_FORCE = 8f;
    [SerializeField] float ACTION_RAYCAST_LENGTH = 1f;
    [SerializeField] float MAX_DISTANCE = 2.1f;
    public float TURNING_COEFF = 3f;
    public float FALLSPEED_BOOST = 1.4f;
    public float SENSITIVITY = 0.2f;
    public float DIRECTION_CORRECTION = 3f;
    public bool CAN_DODGE = false;
    public bool CAN_JUMP = true;
    public bool CAN_ACT = true;
    public bool CAN_CROUCH = false;

    public Vector2 input;

    ColliderInfo current;
    bool queueJump = false;

    public ColliderInfo crouchCollider, standCollider;

    [Tooltip("See comment in code for how to use")]
    // if you have a behavior that needs to be called by the Action func, then
    // in your behavior script, create an OnEnable and OnDisable func that subscribe
    // and desubscribe (respectively) from the delegate.
    public delegate void ActionWithRaycast(RaycastHit r);
    public ActionWithRaycast
        WindowActionDelegate,
        ItemActionDelegate,
        FireballActionDelegate;


    public RaycastHit 
        groundInformation,
        roofInformation,
        actionInformation;
    // additional fields here

    public void ResetData(Vector3 pos)
    {
        Vector3 dest = pos + 0.5f * current.height * Vector3.up;

        Time.fixedDeltaTime = FIXED_TIMESTEP;

        input = Vector2.zero;
        CheckAction(dest, Vector3.zero);
        CheckGround(dest);
        CheckRoof(dest);

        // delegates are handled by Subscribers.
        // dont forget to unsubscribe from the delegate.
        // please don't cause a memory leak. thanks.
        CheckDelegates();
    }

    void CheckDelegates()
    {
        if (WindowActionDelegate?.GetInvocationList().Length > DEL_WARNING_WINDOW
            && ItemActionDelegate?.GetInvocationList().Length > DEL_WARNING_THROW
            && FireballActionDelegate?.GetInvocationList().Length > DEL_WARNING_ACTION)
        {
            Debug.LogError("You might have a memory leak in your PlayerData!");
        }
    }

    public void BoolJump(bool value)
    {
        queueJump = false;

        if (CAN_JUMP)
        {
            queueJump = value;
        }
    }

    public bool IsJumpQueued()
    {
        return queueJump;
    }

    public void SetCurrent(ColliderInfo val)
    {
        current = val;
    }

    public float GetDirectionCorrectionMultiplier(Vector3 moveDir, Vector3 velocity)
    {
        return (-Mathf.Min( Vector3.Dot(moveDir, new Vector3(velocity.x, 0f, velocity.z).normalized), 0f)
            + (1.05f / DIRECTION_CORRECTION))
            * DIRECTION_CORRECTION;
    }

    public bool IsLegalIncline(int angle)
    {
        return angle < MAX_INCLINE_ANGLE;
    }

    public bool IsSpeedCapped(float x, float y)
    {
        return (x * x + y * y) > MAX_MOVE_SPEED * MAX_MOVE_SPEED; // squared magnitude :>
    }

    public bool IsFallSpeedCapped(float y_velo)
    {
        return y_velo > MAX_FALL_SPEED;
    }

    public float GetAccelerationCoeff()
    {
        return ACCELERATION * 15f; // * Time.deltaTime * 35f;
    }

    public float GetJumpForce()
    {
        return JUMP_FORCE;
    }

    public bool CheckGround(Vector3 pos)
    {
        return Physics.Raycast(pos + 0.5f * current.height * Vector3.up, Vector3.down, out groundInformation, MAX_DISTANCE, ~(1 << 2));
    }

    public bool CheckRoof(Vector3 pos)
    {
        return Physics.Raycast(pos - 0.5f * current.height * Vector3.up, Vector3.up, out roofInformation, MAX_DISTANCE, ~(1 << 2));
    }

    public bool CheckAction(Vector3 pos, Vector3 forward)
    {
        return Physics.Raycast(pos, forward, out actionInformation, ACTION_RAYCAST_LENGTH, ~(1 << 2));
    }

    public void FireAction(int index)
    {
        switch(index)
        {
            case 0:
                WindowActionDelegate?.Invoke(actionInformation);
                break;

            case 1:
                ItemActionDelegate?.Invoke(actionInformation);
                break;

            default:
                FireballActionDelegate?.Invoke(actionInformation);
                break;
        }
    }
}

[System.Serializable]
public struct ColliderInfo
{
    public Vector3 center;
    public float radius;
    public float height;
    public int direction;

    public ColliderInfo(Vector3 center, float radius, float height, int direction)
    {
        this.center = center;
        this.radius = radius;
        this.height = height;
        this.direction = direction;
    }
}
