using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SmartCamera2D : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Rigidbody2D targetRb;

    [Header("Follow")]
    public Vector2 baseOffset = Vector2.zero;
    public float smoothTime = 0.15f;
    public bool snapToTargetOnStart = true;

    [Header("Dead Zone")]
    public bool useDeadZone = true;
    public Vector2 deadZoneSize = new Vector2(2.5f, 1.5f);

    [Header("Look Ahead")]
    public bool useLookAhead = true;
    public float lookAheadDistanceX = 2.5f;
    public float lookAheadDistanceY = 1.5f;
    public float lookAheadSmoothTime = 0.12f;
    public float movementThreshold = 0.05f;
    public float returnToCenterSpeed = 2.5f;

    [Header("Fall / Jump Bias")]
    public bool useVerticalFallBias = true;
    public float fallingOffsetY = -1.5f;
    public float fallingSpeedThreshold = -0.1f;
    public float verticalBiasSmoothTime = 0.2f;

    [Header("Bounds")]
    public bool useBounds = false;
    public Vector2 minCameraPosition;
    public Vector2 maxCameraPosition;

    [Header("Debug")]
    public bool drawLookAheadGizmo = true;

    private Camera cam;

    private Vector3 followVelocity;
    private Vector3 lookAheadCurrent;
    private Vector3 lookAheadVelocity;
    private float verticalBiasCurrent;
    private float verticalBiasVelocity;

    private Vector3 lastTargetPosition;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        if (target == null)
            return;

        if (targetRb == null)
            targetRb = target.GetComponent<Rigidbody2D>();

        lastTargetPosition = target.position;

        if (snapToTargetOnStart)
        {
            Vector3 startPos = GetDesiredCameraCenterImmediate();
            transform.position = ClampToBounds(startPos);
        }
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector2 movement = GetTargetMovement();
        UpdateLookAhead(movement);
        UpdateVerticalBias();

        Vector3 desiredCenter = GetDesiredCameraCenter();
        Vector3 nextPosition;

        if (useDeadZone)
        {
            nextPosition = ApplyDeadZone(transform.position, desiredCenter);
        }
        else
        {
            nextPosition = desiredCenter;
        }

        nextPosition.z = transform.position.z;

        Vector3 smoothed = Vector3.SmoothDamp(
            transform.position,
            nextPosition,
            ref followVelocity,
            smoothTime
        );

        transform.position = ClampToBounds(smoothed);

        lastTargetPosition = target.position;
    }

    private Vector2 GetTargetMovement()
    {
        if (targetRb != null)
        {
            return targetRb.linearVelocity;
        }

        Vector3 delta = target.position - lastTargetPosition;
        return delta / Mathf.Max(Time.deltaTime, 0.0001f);
    }

    private void UpdateLookAhead(Vector2 movement)
    {
        if (!useLookAhead)
        {
            lookAheadCurrent = Vector3.zero;
            return;
        }

        Vector3 desiredLookAhead = Vector3.zero;

        if (Mathf.Abs(movement.x) > movementThreshold)
        {
            desiredLookAhead.x = Mathf.Sign(movement.x) * lookAheadDistanceX;
        }
        else
        {
            desiredLookAhead.x = Mathf.MoveTowards(
                lookAheadCurrent.x,
                0f,
                returnToCenterSpeed * Time.deltaTime
            );
        }

        if (Mathf.Abs(movement.y) > movementThreshold)
        {
            desiredLookAhead.y = Mathf.Sign(movement.y) * lookAheadDistanceY;
        }
        else
        {
            desiredLookAhead.y = Mathf.MoveTowards(
                lookAheadCurrent.y,
                0f,
                returnToCenterSpeed * Time.deltaTime
            );
        }

        lookAheadCurrent = Vector3.SmoothDamp(
            lookAheadCurrent,
            desiredLookAhead,
            ref lookAheadVelocity,
            lookAheadSmoothTime
        );
    }

    private void UpdateVerticalBias()
    {
        float targetBias = 0f;

        if (useVerticalFallBias)
        {
            float verticalSpeed = 0f;

            if (targetRb != null)
                verticalSpeed = targetRb.linearVelocity.y;
            else
                verticalSpeed = (target.position.y - lastTargetPosition.y) / Mathf.Max(Time.deltaTime, 0.0001f);

            if (verticalSpeed < fallingSpeedThreshold)
                targetBias = fallingOffsetY;
        }

        verticalBiasCurrent = Mathf.SmoothDamp(
            verticalBiasCurrent,
            targetBias,
            ref verticalBiasVelocity,
            verticalBiasSmoothTime
        );
    }

    private Vector3 GetDesiredCameraCenter()
    {
        Vector3 desired = target.position;
        desired += (Vector3)baseOffset;
        desired += lookAheadCurrent;
        desired.y += verticalBiasCurrent;
        desired.z = transform.position.z;
        return desired;
    }

    private Vector3 GetDesiredCameraCenterImmediate()
    {
        Vector3 desired = target.position + (Vector3)baseOffset;
        desired.z = transform.position.z;
        return desired;
    }

    private Vector3 ApplyDeadZone(Vector3 currentCameraPos, Vector3 desiredTargetPos)
    {
        Vector3 result = currentCameraPos;

        float left = currentCameraPos.x - deadZoneSize.x * 0.5f;
        float right = currentCameraPos.x + deadZoneSize.x * 0.5f;
        float bottom = currentCameraPos.y - deadZoneSize.y * 0.5f;
        float top = currentCameraPos.y + deadZoneSize.y * 0.5f;

        if (desiredTargetPos.x < left)
            result.x = desiredTargetPos.x + deadZoneSize.x * 0.5f;
        else if (desiredTargetPos.x > right)
            result.x = desiredTargetPos.x - deadZoneSize.x * 0.5f;

        if (desiredTargetPos.y < bottom)
            result.y = desiredTargetPos.y + deadZoneSize.y * 0.5f;
        else if (desiredTargetPos.y > top)
            result.y = desiredTargetPos.y - deadZoneSize.y * 0.5f;

        result.z = currentCameraPos.z;
        return result;
    }

    private Vector3 ClampToBounds(Vector3 position)
    {
        if (!useBounds || cam == null || !cam.orthographic)
            return position;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        float minX = minCameraPosition.x + halfWidth;
        float maxX = maxCameraPosition.x - halfWidth;
        float minY = minCameraPosition.y + halfHeight;
        float maxY = maxCameraPosition.y - halfHeight;

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        return position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(
            new Vector3(transform.position.x, transform.position.y, 0f),
            new Vector3(deadZoneSize.x, deadZoneSize.y, 0f)
        );

        if (useBounds)
        {
            Gizmos.color = Color.green;
            Vector2 center = (minCameraPosition + maxCameraPosition) * 0.5f;
            Vector2 size = maxCameraPosition - minCameraPosition;
            Gizmos.DrawWireCube(center, size);
        }

        if (drawLookAheadGizmo && target != null)
        {
            Gizmos.color = Color.cyan;
            Vector3 desired = target.position + (Vector3)baseOffset + lookAheadCurrent;
            Gizmos.DrawLine(target.position, desired);
            Gizmos.DrawWireSphere(desired, 0.15f);
        }
    }
}