using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float runSpeed = 2;
    public float jumpSpeed = 10f;
    public float doubleJumpSpeed = 1f;
    private bool canDoubleJump = false;
    public bool isDead = false;

    [Header("GroundCheck")]
    
    //public Transform groundCheckPos;
    //public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    //public LayerMask groundLayer;
    
    public float fallMultiplier = 2.5f;   // fa caure més ràpid
    public float lowJumpMultiplier = 2f;  // talla el salt si soltes el botó
    Rigidbody2D rb2D;
    bool jumpHeld;
    public SpriteRenderer spriteRenderer; //publica per a dirli des de l'inspector que és el sprite renderer del jugador
    public Animator animator; //publica per a dirli des de l'inspector que és el animator del jugador
    bool isTouchingFront = false;
    bool isWallSliding;
    public float wallSlideSpeed = 0.75f;
    bool isTouchingRight;
    bool isTouchingLeft;
    private string lastWallJumped = ""; // guarda quina paret has saltat per última vegada
    //---Sons de salt---
    public AudioClip jumpSound;
    public AudioClip doubleJumpSound;
    //---Sons de salt---
    //---Disparar---
    public ProjectileBehaviour projectilePrefab;
    public Transform launchOffSet;
    public bool canShoot = false;
    private float shootCooldown = 1f;
    private float lastShotTime = -1f;
    public AudioClip shootSound;
    //---Disparar---
    //---So de dany---
    public AudioClip hurtSound;
    //---So de dany---
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        canShoot = GameManager.hasShootPower;
    }

    void FixedUpdate()
    {
        if (isDead) return; // Si està mort, no fa res

        //---Moviment horitzontal---
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            rb2D.linearVelocity = new Vector2(runSpeed, rb2D.linearVelocity.y);
            spriteRenderer.flipX = false;
            animator.SetBool("Run", true);
        }
        else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            rb2D.linearVelocity = new Vector2(-runSpeed, rb2D.linearVelocity.y);
            spriteRenderer.flipX = true;
            animator.SetBool("Run", true);
        }
        else
        {
            rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
            animator.SetBool("Run", false);
        }
        //---Moviment horitzontal---

        //-----Salt variable------
        if (rb2D.linearVelocity.y < 0)
        {
            rb2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb2D.linearVelocity.y > 0 && !jumpHeld)
        {
            rb2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
        //-----Salt variable------
    }

    void Update()
    {
        if (isDead) return;

        jumpHeld = Keyboard.current.wKey.isPressed || 
                Keyboard.current.spaceKey.isPressed || 
                Keyboard.current.upArrowKey.isPressed;

        bool jumpPressed = Keyboard.current.wKey.wasPressedThisFrame || 
                        Keyboard.current.spaceKey.wasPressedThisFrame || 
                        Keyboard.current.upArrowKey.wasPressedThisFrame;

        //---Wall Sliding (primer de tot)---
        if (isTouchingFront && !CheckGround.isGrounded)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            animator.SetBool("Falling", false);
            animator.Play("Wall");
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, Mathf.Clamp(rb2D.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
        //---Wall Sliding---

        //-----Salts------
        if (jumpPressed && isWallSliding)
        {
            if (isTouchingRight && lastWallJumped != "WallRight")
            {
                lastWallJumped = "WallRight";
                isWallSliding = false;
                animator.SetBool("Jump", true);
                rb2D.linearVelocity = new Vector2(-runSpeed * 2, jumpSpeed);
                spriteRenderer.flipX = true;
                if (jumpSound != null)
                {
                    AudioSource.PlayClipAtPoint(jumpSound, transform.position);   
                }
            }
            else if (isTouchingLeft && lastWallJumped != "WallLeft")
            {
                lastWallJumped = "WallLeft";
                isWallSliding = false;
                animator.SetBool("Jump", true);
                rb2D.linearVelocity = new Vector2(runSpeed * 2, jumpSpeed);
                spriteRenderer.flipX = false;

                if (jumpSound != null)
                {
                    AudioSource.PlayClipAtPoint(jumpSound, transform.position);   
                }
            }
        }
        else if (jumpPressed && CheckGround.isGrounded)
        {
            canDoubleJump = true;
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpSpeed);
            animator.SetBool("Jump", true);
            animator.SetBool("Run", false);
            if (jumpSound != null)
            {
                AudioSource.PlayClipAtPoint(jumpSound, transform.position);
            }
        }
        else if (jumpPressed && canDoubleJump)
        {
            animator.SetBool("DoubleJump", true);
            animator.SetBool("Jump", false);
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, doubleJumpSpeed);
            canDoubleJump = false;
            if (doubleJumpSound != null)
            {
                AudioSource.PlayClipAtPoint(doubleJumpSound, transform.position);
            }
        }
        //-----Salts------

        if (CheckGround.isGrounded)
        {
            lastWallJumped = "";
            animator.SetBool("Jump", false);
            animator.SetBool("DoubleJump", false);
            animator.SetBool("Falling", false);
        }
        else
        {
            animator.SetBool("Jump", true);
            animator.SetBool("Run", true);
        }

        if (rb2D.linearVelocity.y < 0 && !isWallSliding)
        {
            animator.SetBool("Falling", true);
        }
        else if (rb2D.linearVelocity.y > 0 && !jumpHeld)
        {
            animator.SetBool("Falling", false);
        }

        bool jumpReleased = Keyboard.current.wKey.wasReleasedThisFrame || 
                            Keyboard.current.spaceKey.wasReleasedThisFrame || 
                            Keyboard.current.upArrowKey.wasReleasedThisFrame;

        if (jumpReleased && rb2D.linearVelocity.y > 0)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, rb2D.linearVelocity.y * 0.5f);
        }

        //---Disparar---
        if ((Keyboard.current.jKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame) && canShoot)
        {
            if (Time.time - lastShotTime >= shootCooldown)
            {
                lastShotTime = Time.time;

                bool goingRight = !spriteRenderer.flipX;

                Vector3 offset = launchOffSet.localPosition;
                offset.x = Mathf.Abs(offset.x) * (goingRight ? 1 : -1);
                launchOffSet.localPosition = offset;

                Quaternion rotation = goingRight ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90);

                ProjectileBehaviour bullet = Instantiate(projectilePrefab, launchOffSet.position, rotation);
                bullet.goingRight = goingRight;
                if (shootSound != null)
                {
                    AudioSource.PlayClipAtPoint(shootSound, transform.position);   
                }
            }
        }
        //---Disparar---
    }

    public void Die()
    {
        isDead = true;
        animator.SetBool("Hit", true);
        rb2D.linearVelocity = Vector2.zero;
        rb2D.gravityScale = 0f;
        //rb2D.isKinematic = true;
        if (hurtSound != null)
        {
            AudioSource.PlayClipAtPoint(hurtSound, transform.position, 1000f);
        }
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        LifeManager.LoseLife();
        if (LifeManager.lives <= 0)
        {
            LifeManager.lives = 3; // reseteja les vides
            RespawnManager.respawnPoints.Clear(); // reseteja els punts de respawn
            RespawnManager.lastCheckpointID = ""; // reseteja el checkpoint
            GameManager.ResetFruits(); // reseteja les fruites
            GameManager.ResetEnemies(); // reseteja els enemics
            GameManager.hasShootPower = false; // reseteja el poder de disparar
            GameManager.bossHitPoints = -1f; // reseteja la vida del boss
            SceneManager.LoadScene(0); // torna al menú
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    //---Wall Sliding---
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WallRight"))
        {
            isTouchingFront = true;
            isTouchingRight = true;
        }

        if (collision.gameObject.CompareTag("WallLeft"))
        {
            isTouchingFront = true;
            isTouchingLeft = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isTouchingFront = false;
        isTouchingRight = false;
        isTouchingLeft = false;
    }
    //---Wall Sliding---
}