using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && LifeManager.lives < 3)
        {
            LifeManager.lives++;
            Destroy(gameObject);
        }
    }
}