using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    private BoxCollider2D boundsCollider;

    public Vector2 MinBound => boundsCollider.bounds.min;
    public Vector2 MaxBound => boundsCollider.bounds.max;

    private void Awake()
    {
        boundsCollider = GetComponent<BoxCollider2D>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (GetComponent<BoxCollider2D>())
        {
            Gizmos.DrawWireCube(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size);
        }
    }
}
