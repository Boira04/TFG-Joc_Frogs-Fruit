// BadBullet.cs
// Projectil de foc que persegueix el jugador continuament
// Utilitzat tant pel del passadis com pel FinalBossController (atac1)
// En instanciarse, ignora la colisio amb els enemics MaskDudes per evitar bloquejos
using UnityEngine;
public class BadBullet : MonoBehaviour
{
    public float speed = 4f;
    private Transform target;
    private Rigidbody2D rb;
    public AudioClip fireSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Evita que la bala caigui per gravetat

        if (fireSound != null)
            AudioSource.PlayClipAtPoint(fireSound, transform.position);

        // Ignora colisio amb tots els maskdudes (enemics spawnjeats amb ParabolicBullet) actius perque els pugui travessar
        GameObject[] spawnedEnemies = GameObject.FindGameObjectsWithTag("SpawnedEnemy");
        foreach (GameObject enemy in spawnedEnemies)
        {
            Collider2D enemyCol = enemy.GetComponent<Collider2D>();
            if (enemyCol != null)
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemyCol);
        }
    }

    // Assigna el jugador com a objectiu,sha de cridar just despres dinstanciar la bala
    public void Init(Transform playerTransform)
    {
        target = playerTransform;
    }

    private void FixedUpdate()
    {
        if (target == null) { Destroy(gameObject); return; }
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BadBullet")) return; // Evita que les bales es destrueixin entre elles

        if (collision.CompareTag("Player"))
        {
            PlayerMove player = collision.GetComponent<PlayerMove>();
            if (player != null && !player.isDead) player.Die();
            Destroy(gameObject);
        }
        else if (!collision.isTrigger)
        {
            Destroy(gameObject); // Es destrueix en tocar qualsevol superficie solida
        }
    }
}