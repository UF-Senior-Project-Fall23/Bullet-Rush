using UnityEngine;

[ExecuteInEditMode]
public class FollowObject : MonoBehaviour
{
    public Transform target;  // The object to follow
    public float offsetX = 0f;  // Offset in the X-axis
    public float offsetY = 0f;  // Offset in the Y-axis
    public bool followRotation = false;
    
    void Update()
    {
        if (target is not null)
        {
            Vector3 targetPosition = target.position + new Vector3(offsetX, offsetY, 0f);
            transform.position = targetPosition;
            if(followRotation) transform.rotation = target.rotation;
        }
    }
}