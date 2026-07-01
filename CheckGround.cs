// CheckGround.cs
// Detecta si el jugador toca terra mitjançant un Trigger situat als peus del personatge
// Inclou un petit "coyote time" per evitar que isGrounded canvi massa bruscament
using UnityEngine;
public class CheckGround : MonoBehaviour
{
    public static bool isGrounded;
    private float coyoteTime = 0.05f;
    private float coyoteTimer;

    private void OnTriggerEnter2D(Collider2D collision) { isGrounded = true; coyoteTimer = 0f; }
    private void OnTriggerExit2D(Collider2D collision) { coyoteTimer = 0f; }

    private void Update()
    {
        if (!isGrounded) return;
        coyoteTimer += Time.deltaTime;
        if (coyoteTimer >= coyoteTime)
        {
            if (!GetComponent<Collider2D>().IsTouchingLayers()) { isGrounded = false; coyoteTimer = 0f; }
        }
    }
}