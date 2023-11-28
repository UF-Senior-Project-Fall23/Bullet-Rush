using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private static readonly float HalfWidth = Screen.width / 2f;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        
        //Debug.Log($"Mouse Pos: {Input.mousePosition.x} | Normal Width: {Screen.width} | Half Width: {HalfWidth} | {Input.mousePosition.x >= HalfWidth}");

        var lookX = 2 * System.Convert.ToSingle(Camera.main.ScreenToWorldPoint(Input.mousePosition).x > gameObject.transform.position.x) - 1;
        
        
        animator.SetFloat("X", lookX);
        animator.SetBool("IsMoving", x != 0 || y != 0);
        
    }
}
