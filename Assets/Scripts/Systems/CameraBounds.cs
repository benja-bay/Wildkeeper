using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CameraBounds : MonoBehaviour
{
    private Bounds _bounds;

    public Vector2 MinBound => _bounds.min;
    public Vector2 MaxBound => _bounds.max;

    private void Awake()
    {
        var col = GetComponent<Collider2D>();
        if (col != null)
        {
            _bounds = col.bounds;
        }
        else
        {
            Debug.LogError("CameraBounds requiere un Collider2D.");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        }
    }
}
