// DamageObject.cs
// Objecte que fa morir el jugador en colisionar amb aquest (spikes)
using UnityEngine;
public class DamageObject : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerMove player = FindFirstObjectByType<PlayerMove>();
            if (!player.isDead)
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
                player.Die();
            }
        }
    }
}