using UnityEngine;

public class ParabolicBullet : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    public AudioClip fireSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (fireSound != null)
        {
            AudioSource.PlayClipAtPoint(fireSound, transform.position);
        }
    }

    public void Launch(Vector2 initialVelocity)
    {
        rb.linearVelocity = initialVelocity;
    }

    private void Update()
    {
        // Comprova si hi ha terra just a sota abans d'arribar-hi
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        if (hit.collider != null)
        {
            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
            if (player != null && !player.isDead)
            {
                player.Die();
            }
            Explode();
        }
    }

    private void Explode()
    {
        if (enemyToSpawn != null)
        {
            GameObject enemy = Instantiate(enemyToSpawn, transform.position, Quaternion.identity);

            // ignorar la col·lisió entre el maskdude i el boss
            FinalBossController boss = FindFirstObjectByType<FinalBossController>();
            if (boss != null)
            {
                Physics2D.IgnoreCollision(
                    enemy.GetComponent<Collider2D>(),
                    boss.GetComponent<Collider2D>()
                );
            }
        }
        Destroy(gameObject);
    }
}