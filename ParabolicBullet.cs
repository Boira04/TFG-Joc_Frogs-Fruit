// ParabolicBullet.cs
// Projectil que segueix una trajectoria parabolica (afectat per la gravetat)
// En detectar terra amb un Raycast, explota i spawnejа un enemic al seu lloc
// Els enemics spawnejats ignoren la colisio amb el boss per no quedarse encallats
using UnityEngine;
public class ParabolicBullet : MonoBehaviour
{
    public GameObject enemyToSpawn;         // Enemic que apareix en explotar (prefab amb enemyID buit = temporal)
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;           // Ha dapuntar a la Layer "Ground" (no Default)
    public AudioClip fireSound;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (fireSound != null) AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }

    public void Launch(Vector2 initialVelocity) => rb.linearVelocity = initialVelocity;

    private void Update()
    {
        // Explota abans de tocar el terra per evitar bugs de rebot
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        if (hit.collider != null) Explode();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
            if (player != null && !player.isDead) player.Die();
            Explode();
        }
    }

    private void Explode()
    {
        if (enemyToSpawn != null)
        {
            GameObject enemy = Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
            FinalBossController boss = FindFirstObjectByType<FinalBossController>();
            if (boss != null)
                Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), boss.GetComponent<Collider2D>());
        }
        Destroy(gameObject);
    }
}