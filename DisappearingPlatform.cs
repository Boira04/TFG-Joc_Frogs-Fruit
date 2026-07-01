using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public float delayBeforeOff = 0.5f;
    public float timeToRespawn = 2f;
    private Animator animator;
    private Collider2D col;
    private bool isDisappearing = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        animator.SetBool("isOn", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDisappearing)
        {
            // Comprova si el contacte és des de dalt
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    StartCoroutine(DisappearRoutine());
                    break;
                }
            }
        }
    }

    IEnumerator DisappearRoutine()
    {
        isDisappearing = true;

        yield return new WaitForSeconds(delayBeforeOff);

        animator.SetBool("isOn", false);
        col.isTrigger = true;

        yield return new WaitForSeconds(timeToRespawn);

        col.isTrigger = false;
        animator.SetBool("isOn", true);
        isDisappearing = false;
    }
}