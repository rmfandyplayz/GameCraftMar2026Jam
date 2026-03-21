using UnityEngine;

public class DynamicCamera2D : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow")]
    public float smoothTime = 0.15f;
    public Vector2 offset;

    [Header("Dead Zone")]
    public Vector2 deadZoneSize = new Vector2(2f, 1.5f);

    [Header("Look Ahead")]
    public bool useLookAhead = true;
    public float lookAheadAmount = 1.5f;
    public float lookAheadSmoothTime = 0.2f;
    public float lookAheadMoveThreshold = 0.05f;

    [Header("Camera Bounds")]
    public bool useBounds = true;
    public Vector2 minCameraPosition;
    public Vector2 maxCameraPosition;

    private Vector3 velocity;
    private Vector3 currentLookAhead;
    private Vector3 lookAheadVelocity;
    private Vector3 lastTargetPosition;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        if (target != null)
        {
            lastTargetPosition = target.position;

            Vector3 startPos = transform.position;
            startPos.x = target.position.x + offset.x;
            startPos.y = target.position.y + offset.y;
            startPos.z = transform.position.z;
            transform.position = ClampToBounds(startPos);
        }
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 cameraPos = transform.position;
        Vector3 targetPos = target.position + (Vector3)offset;

        Vector3 targetDelta = target.position - lastTargetPosition;

        Vector3 desiredLookAhead = Vector3.zero;

        if (useLookAhead)
        {
            if (Mathf.Abs(targetDelta.x) > lookAheadMoveThreshold)
            {
                desiredLookAhead.x = Mathf.Sign(targetDelta.x) * lookAheadAmount;
            }
            else
            {
                desiredLookAhead.x = 0f;
            }

            currentLookAhead = Vector3.SmoothDamp(
                currentLookAhead,
                desiredLookAhead,
                ref lookAheadVelocity,
                lookAheadSmoothTime
            );
        }
        else
        {
            currentLookAhead = Vector3.zero;
        }

        targetPos += currentLookAhead;

        float deadZoneLeft = cameraPos.x - deadZoneSize.x * 0.5f;
        float deadZoneRight = cameraPos.x + deadZoneSize.x * 0.5f;
        float deadZoneBottom = cameraPos.y - deadZoneSize.y * 0.5f;
        float deadZoneTop = cameraPos.y + deadZoneSize.y * 0.5f;

        Vector3 newTargetPos = cameraPos;

        if (targetPos.x < deadZoneLeft)
        {
            newTargetPos.x = targetPos.x + deadZoneSize.x * 0.5f;
        }
        else if (targetPos.x > deadZoneRight)
        {
            newTargetPos.x = targetPos.x - deadZoneSize.x * 0.5f;
        }

        if (targetPos.y < deadZoneBottom)
        {
            newTargetPos.y = targetPos.y + deadZoneSize.y * 0.5f;
        }
        else if (targetPos.y > deadZoneTop)
        {
            newTargetPos.y = targetPos.y - deadZoneSize.y * 0.5f;
        }

        newTargetPos.z = transform.position.z;

        Vector3 smoothedPos = Vector3.SmoothDamp(
            transform.position,
            newTargetPos,
            ref velocity,
            smoothTime
        );

        transform.position = ClampToBounds(smoothedPos);

        lastTargetPosition = target.position;
    }

    private Vector3 ClampToBounds(Vector3 position)
    {
        if (!useBounds)
            return position;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        position.x = Mathf.Clamp(position.x, minCameraPosition.x + halfWidth, maxCameraPosition.x - halfWidth);
        position.y = Mathf.Clamp(position.y, minCameraPosition.y + halfHeight, maxCameraPosition.y - halfHeight);

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
    }
}