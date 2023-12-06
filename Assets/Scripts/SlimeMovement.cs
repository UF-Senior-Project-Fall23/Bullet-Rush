using UnityEngine;

// Code for a deprecated enemy
// TODO: Remove?
public class SlimeMovement : MonoBehaviour
{
    [SerializeField] private GameObject target;

    // Update is called once per frame
    void Update()
    {
        var distVal = 5.0f;
        var dis = Vector3.Distance(transform.position, target.transform.position);
        if (dis <= distVal)
        {

        }
    }
}
