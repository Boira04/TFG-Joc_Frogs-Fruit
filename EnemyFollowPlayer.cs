using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float speed;
    public float lineOfSite;
    public AudioClip detectSound; // Nou camp
    private Transform player;
    private Animator animator;
    public bool canMove = true;
    private bool playerDetected = false; // Evita que el so es reprodueixi cada frame

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSite && canMove)
        {
            if (!playerDetected)
            {
                playerDetected = true;
                if (detectSound != null)
                {
                    AudioSource.PlayClipAtPoint(detectSound, transform.position);
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            animator.SetBool("Run", true);
            if (player.position.x < transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            animator.SetBool("Run", false);
            playerDetected = false; // Reseteja si el jugador surt del rang
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
    }
}