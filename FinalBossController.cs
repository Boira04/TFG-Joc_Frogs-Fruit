using System.Collections;
using UnityEngine;

public class FinalBossController : MonoBehaviour, IDamageable
{
    [Header("Vida")]
    public float hitPoints;
    public float maxHitPoints = 20;

    [Header("Atacs")]
    public float timeBetweenAttacks = 2.5f;
    private bool canAttack = true;
    private bool isDead = false;

    private Animator animator;
    private Transform player;
    //---ATAAAAAAACS---
    //---ATAC 1: Foc---
    [Header("Atac 1: Foc")]
    public GameObject fireBulletPrefab;
    public Transform fireSpawnPoint;
    //---ATAC 1: Foc---
    //---ATAC 2: Bala parabola---
    [Header("Atac 2: Bala paràbola")]
    public GameObject parabolicBulletPrefab;
    public Transform parabolicSpawnPoint;
    public float launchSpeedX = 5f;
    public float launchSpeedY = 8f;
    public int maxSpawnedEnemies = 2;
    public string spawnedEnemyTag = "SpawnedEnemy";
    //---ATAC 2: Bala parabola---
    //---ATAC 3: Embestida---
    [Header("Atac 3: Embestida")]
    public float dashSpeed = 8f;
    public float dashDuration = 0.6f;
    private Rigidbody2D rb;
    public AudioClip dashSound;
    //---ATAC 3: Embestida---
    private Vector3 originalScale;
    //---MORT---
    [Header("Mort")]
    public AudioClip deathSound;
    public AudioClip explosionSound;
    public AudioClip victoryMusic;
    public GameObject fireworksPrefab;
    public float timeBeforeNextScene = 10f;
    Vector3 fireworksPosition = new Vector3(-2.99f, 3.94f, 0f); // en una posicio fixa on es vegi en la pantalla, no a la del boss
    //---MORT---
    private bool isInvulnerable = false;
    void Start()
    {
        // recupera la vida guardada si existeix
        if (GameManager.bossHitPoints > 0)
        {
            hitPoints = GameManager.bossHitPoints;
        }
        else
        {
            hitPoints = maxHitPoints;
        }

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalScale = transform.localScale;

        GameObject[] platforms = GameObject.FindGameObjectsWithTag("FallingPlatform");
        foreach (GameObject platform in platforms)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), platform.GetComponent<Collider2D>());
        }

        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);

            if (!canAttack) continue;

            int attackIndex;
            int currentEnemies = GameObject.FindGameObjectsWithTag(spawnedEnemyTag).Length;

            if (currentEnemies >= maxSpawnedEnemies)
            {
                // Nomes pot fer latac 1 o 3 si hi ha el maxim denemics vius
                attackIndex = Random.Range(0, 2) == 0 ? 1 : 3;
            }
            else
            {
                attackIndex = Random.Range(1, 4);
            }

            animator.SetInteger("attackIndex", attackIndex);
            animator.SetTrigger("triggerAttack");

            if (attackIndex == 1)
            {
                StartCoroutine(FireAttackDelayed(0.4f));
            }
            else if (attackIndex == 2)
            {
                StartCoroutine(ParabolicAttackDelayed(0.4f));
            }
            else if (attackIndex == 3)
            {
                StartCoroutine(DashAttack());
            }

            yield return new WaitForSeconds(GetAttackDuration(attackIndex));
        }
    }

    float GetAttackDuration(int index)
    {
        switch (index)
        {
            case 1: return 1.5f;
            case 2: return 1.5f;
            case 3: return 1f;
            default: return 1f;
        }
    }

    IEnumerator ResetHit()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Hit", false);
        canAttack = true;
    }
    //---MORT---
    void Die()
    {
        isDead = true;
        StopAllCoroutines();

        animator.SetBool("Hit", false);
        animator.SetBool("isDead", true);
        rb.linearVelocity = Vector2.zero;

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        GameManager.bossHitPoints = -1f;
        CleanUpAttacks(); // neteja les atacs actius en morir el final boss
        StartCoroutine(DeathRoutine());
    }

    void CleanUpAttacks()
    {
        // Destrueix totes les BadBullet actives (foc)
        BadBullet[] badBullets = FindObjectsByType<BadBullet>(FindObjectsSortMode.None);
        foreach (BadBullet bullet in badBullets)
        {
            Destroy(bullet.gameObject);
        }

        // Destrueix totes les ParabolicBullet actives (encara en l'aire)
        ParabolicBullet[] parabolicBullets = FindObjectsByType<ParabolicBullet>(FindObjectsSortMode.None);
        foreach (ParabolicBullet bullet in parabolicBullets)
        {
            Destroy(bullet.gameObject);
        }

        // Opcional: tambe destruir els maskdudes ja spawnejats
        GameObject[] spawnedEnemies = GameObject.FindGameObjectsWithTag(spawnedEnemyTag);
        foreach (GameObject enemy in spawnedEnemies)
        {
            Destroy(enemy);
        }
    }

    IEnumerator DeathRoutine()
    {
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("Death") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
        );

        if (MusicManager.instance != null)
            MusicManager.instance.StopMusic();

        if (fireworksPrefab != null)
            Instantiate(fireworksPrefab, fireworksPosition, Quaternion.identity);

        // Amaguem el boss en lloc de destruir-lo, perque la corrutina pugui continuar
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        if (MusicManager.instance != null && victoryMusic != null)
            MusicManager.instance.PlayMusic(victoryMusic);

        yield return new WaitForSeconds(timeBeforeNextScene);

        int nextScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
    }
    //---MORT---
    //---ATAC 1: Foc---
    void FireAttack()
    {
        if (fireBulletPrefab == null || player == null) return;

        Vector3 spawnPos = fireSpawnPoint != null ? fireSpawnPoint.position : transform.position;

        GameObject b = Instantiate(fireBulletPrefab, spawnPos, Quaternion.identity);
        BadBullet badBullet = b.GetComponent<BadBullet>();
        if (badBullet != null)
        {
            badBullet.Init(player);
        }
    }
    IEnumerator FireAttackDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        FireAttack();
    }
    //---ATAC 1: Foc---

    //---ATAC 2: Bala parabola---
    public void ParabolicAttack()
    {
        if (parabolicBulletPrefab == null || player == null) return;

        // Compta quants MaskDudes spawnejats hi ha vius
        int currentEnemies = GameObject.FindGameObjectsWithTag(spawnedEnemyTag).Length;
        if (currentEnemies >= maxSpawnedEnemies) return; // No dispara si ja hi ha el maxim

        Vector3 spawnPos = parabolicSpawnPoint != null ? parabolicSpawnPoint.position : transform.position;
        GameObject b = Instantiate(parabolicBulletPrefab, spawnPos, Quaternion.identity);
        ParabolicBullet bullet = b.GetComponent<ParabolicBullet>();
        float direction = player.position.x > transform.position.x ? 1f : -1f;
        bullet.Launch(new Vector2(direction * launchSpeedX, launchSpeedY));
    }
    IEnumerator ParabolicAttackDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        ParabolicAttack();
    }
    //---ATAC 2: Bala parabola---

    //---ATAC 3: Embestida---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. PROJECTIL DEL JUGADOR
        if (collision.gameObject.CompareTag("Projectile") && !isDead && !isInvulnerable)
        {
            // Mirem si lAnimator esta reproduint algun dels 3 estats datac
            bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
                            animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") ||
                            animator.GetCurrentAnimatorStateInfo(0).IsName("Walk");

            hitPoints--;
            GameManager.bossHitPoints = hitPoints; 
            GetComponentInChildren<HealthBarBehaviour>()?.ShowBar(); 

            if (!isAttacking)
            {
                animator.SetBool("Hit", true);
                canAttack = false;
                StartCoroutine(ResetHit());
            }

            if (hitPoints <= 0)
            {
                Die();
            }
        }

        // 2. CONTACTE COS A COS AMB EL JUGADOR
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
            if (player != null && !player.isDead)
            {
                player.Die();
            }

            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
            StartCoroutine(ReEnableCollisionWithPlayer(collision.collider));
        }
    }
    private int dashDirection = 1; // comença mirant a la dreta
    IEnumerator DashAttack()
    {
        dashDirection *= -1; // alterna entre 1 i -1 cada embestida per tal de "mirar sempre" al jugador

        transform.localScale = new Vector3(
            Mathf.Abs(originalScale.x) * dashDirection,
            originalScale.y,
            originalScale.z
        );

        if (dashSound != null)
        {
            AudioSource.PlayClipAtPoint(dashSound, transform.position);
        }

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
    }
    IEnumerator ReEnableCollisionWithPlayer(Collider2D playerCollider)
    {
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>(), false);
    }
    //---ATAC 3: Embestida---

    // Funcions de la interficie IDamageable
    public float GetHitPoints() => hitPoints;
    public float GetMaxHitPoints() => maxHitPoints;
    // Funcions de la interficie IDamageable
}