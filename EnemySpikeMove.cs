using UnityEngine;
public class EnemySpikeMove : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            PlayerMove player = FindFirstObjectByType<PlayerMove>();
            
            if (!player.isDead) // Només crida Die() si no està ja mort
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
                player.Die();
            }
        }
    }
}