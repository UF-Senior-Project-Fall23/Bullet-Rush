using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpPlayerAim : MonoBehaviour
{

    public Transform player; // Reference to the player GameObject
    public float followSpeed; // Adjust the speed of following
    public float maxDistX; // Maximum distance on the X-axis
    public float maxDistY; // Maximum distance on the Y-axis

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 midwayPoint = (player.position + mousePosition) / 2;

        // Calculate the distance between the player and the midway point
        float distX = Mathf.Abs(midwayPoint.x - player.position.x);
        float distY = Mathf.Abs(midwayPoint.y - player.position.y);

        // Clamp the distances to the specified maximums
        float calcCamX = Mathf.Clamp(distX, 0f, maxDistX);
        float calcCamY = Mathf.Clamp(distY, 0f, maxDistY);

        // Calculate the new midway point with clamped distances
        midwayPoint = new Vector3(
            player.position.x + (mousePosition.x - player.position.x > 0 ? calcCamX : -calcCamX),
            player.position.y + (mousePosition.y - player.position.y > 0 ? calcCamY : -calcCamY),
            midwayPoint.z
        );

        // Move the follower towards the new midway point
        transform.position = Vector3.Lerp(transform.position, midwayPoint, followSpeed * Time.deltaTime);
    }

}
