using UnityEngine;

public class ThwompBehaviour : MonoBehaviour
{
    public float fallSpeed = 10f;
    public float riseSpeed = 3f;
    public float detectionRange = 5f;
    public AudioClip impactSound; // Nou camp
    private Rigidbody2D rb2D;
    private Animator animator;
    private Vector3 startPosition;
    private Transform player;
    private float blinkTimer = 0f;
    public float blinkInterval = 2f;
    private enum State { Idle, Falling, HitGround, Rising }
    private State currentState = State.Idle;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                blinkTimer += Time.deltaTime;
                if (blinkTimer >= blinkInterval)
                {
                    blinkTimer = 0f;
                    StartCoroutine(BlinkAnimation());
                }
                float distanceX = Mathf.Abs(player.position.x - transform.position.x);
                if (distanceX < detectionRange)
                {
                    currentState = State.Falling;
                }
                break;
            case State.Falling:
                rb2D.linearVelocity = new Vector2(0, -fallSpeed);
                break;
            case State.Rising:
                rb2D.linearVelocity = new Vector2(0, riseSpeed);
                if (transform.position.y >= startPosition.y)
                {
                    rb2D.linearVelocity = Vector2.zero;
                    transform.position = startPosition;
                    currentState = State.Idle;
                }
                break;
        }
    }

    System.Collections.IEnumerator BlinkAnimation()
    {
        animator.Play("Blink");
        yield return new WaitForSeconds(0.5f);
        animator.Play("Idle");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == State.Falling)
        {
            rb2D.linearVelocity = Vector2.zero;
            animator.SetBool("BottomHit", true);
            currentState = State.HitGround;

            if (impactSound != null)
            {
                AudioSource.PlayClipAtPoint(impactSound, transform.position);
            }

            StartCoroutine(RiseUp());
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
            if (player != null && !player.isDead)
                player.Die();
        }
    }

    System.Collections.IEnumerator RiseUp()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("BottomHit", false);
        currentState = State.Rising;
    }
}