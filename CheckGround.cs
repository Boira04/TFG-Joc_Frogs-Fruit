using UnityEngine;
public class CheckGround : MonoBehaviour
{
    public static bool isGrounded;
    private float coyoteTime = 0.05f; // petit delay
    private float coyoteTimer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        coyoteTimer = 0f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        coyoteTimer = 0f;
    }

    private void Update()
    {
        if (!isGrounded) return;
        coyoteTimer += Time.deltaTime;
        if (coyoteTimer >= coyoteTime)
        {
            // comprova si realment segueix tocant alguna cosa
            if (!GetComponent<Collider2D>().IsTouchingLayers())
            {
                isGrounded = false;
                coyoteTimer = 0f;
            }
        }
    }
}