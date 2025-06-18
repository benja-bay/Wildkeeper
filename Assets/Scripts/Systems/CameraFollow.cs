using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector2 offset;
    [SerializeField] private CameraBounds cameraBounds;

    private float camHalfWidth;
    private float camHalfHeight;

    private void Start()
    {
        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = cam.aspect * camHalfHeight;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + (Vector3)offset;

        if (cameraBounds != null)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, cameraBounds.MinBound.x + camHalfWidth, cameraBounds.MaxBound.x - camHalfWidth);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, cameraBounds.MinBound.y + camHalfHeight, cameraBounds.MaxBound.y - camHalfHeight);
        }

        desiredPosition.z = -10f;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
