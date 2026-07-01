// EnemyBehaviour.cs
// Gestiona la vida i les col·lisions dels enemics normals
// Implementa IDamageable per compatibilitat amb HealthBarBehaviour
// Si enemyID és buit, l'enemic és temporal i no guarda estat al GameManager
using UnityEngine;
public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    public float hitPoints;
    public float maxHitPoints = 5;
    public string enemyID; // ID unic per a cada enemic; buit = enemic temporal (spawnejat pel boss)
    public AudioClip deathSound;
    private Animator animator;
    private bool isInvulnerable = false;
    private EnemyFollowPlayer enemyFollow;

    void Start()
    {
        hitPoints = maxHitPoints;
        animator = GetComponent<Animator>();
        enemyFollow = GetComponent<EnemyFollowPlayer>();
        if (string.IsNullOrEmpty(enemyID)) return;
        var state = GameManager.GetEnemyState(enemyID);
        if (state.HasValue)
        {
            if (state.Value.hitPoints <= 0) { Destroy(gameObject); return; }
            hitPoints = state.Value.hitPoints;
            transform.position = state.Value.position;
        }
    }

    void SaveState()
    {
        if (string.IsNullOrEmpty(enemyID)) return;
        GameManager.SaveEnemy(enemyID, hitPoints, transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && !isInvulnerable)
        {
            hitPoints--;
            SaveState();
            GetComponentInChildren<HealthBarBehaviour>()?.ShowBar();
            animator.SetBool("Hit", true);
            StartCoroutine(ResetHit());
            if (hitPoints <= 0)
            {
                if (!string.IsNullOrEmpty(enemyID)) GameManager.SaveEnemy(enemyID, 0, transform.position);
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (player != null && !player.isDead)
            {
                float diff = collision.transform.position.y - transform.position.y;
                bool jumpedOnTop = diff > 0.15f;
                if (jumpedOnTop && !isInvulnerable)
                {
                    hitPoints--;
                    GetComponentInChildren<HealthBarBehaviour>()?.ShowBar();
                    SaveState();
                    animator.SetBool("Hit", true);
                    StartCoroutine(ResetHit());
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 3f);
                    StartCoroutine(ReEnableCollision(collision.collider));
                    if (hitPoints <= 0)
                    {
                        if (!string.IsNullOrEmpty(enemyID)) GameManager.SaveEnemy(enemyID, 0, transform.position);
                        Destroy(gameObject);
                    }
                }
                else if (!jumpedOnTop)
                {
                    Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
                    player.Die();
                }
            }
        }
    }

    System.Collections.IEnumerator ResetHit()
    {
        isInvulnerable = true;
        enemyFollow.canMove = false;
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Hit", false);
        isInvulnerable = false;
        enemyFollow.canMove = true;
        enemyFollow.speed += 0.5f; // enemic saccelera lleugerament cada cop que rep un cop
    }

    System.Collections.IEnumerator ReEnableCollision(Collider2D playerCollider)
    {
        Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>(), true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>(), false);
    }

    public float GetHitPoints() => hitPoints;
    public float GetMaxHitPoints() => maxHitPoints;
}