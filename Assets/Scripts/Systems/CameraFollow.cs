using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector2 offset;
    [SerializeField] private CameraBounds cameraBounds;
    
    private float camHalfWidth;
    private float camHalfHeight;
    private GameObject _player;
    public static CameraFollow Instance {get; private set;}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = cam.aspect * camHalfHeight;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        if (_player == null) return;

        Vector3 desiredPosition = _player.transform.position + (Vector3)offset;

        if (cameraBounds != null)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, cameraBounds.MinBound.x + camHalfWidth, cameraBounds.MaxBound.x - camHalfWidth);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, cameraBounds.MinBound.y + camHalfHeight, cameraBounds.MaxBound.y - camHalfHeight);
        }

        desiredPosition.z = -10f;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
