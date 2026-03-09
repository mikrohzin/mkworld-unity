using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Offset")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 14f, -12f);

    private void LateUpdate()
    {
        if (target == null)
            return;

        transform.position = target.position + offset;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}