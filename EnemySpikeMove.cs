// EnemySpikeMove.cs
// Spike mòbil que mata el jugador en col·lisionar. Idèntic a DamageObject però
// separat per facilitar l'assignació a objectes en moviment
using UnityEngine;
public class EnemySpikeMove : MonoBehaviour
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