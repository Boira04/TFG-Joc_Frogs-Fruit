// DisappearingPlatform.cs
// Plataforma que desapareix uns instants despres de ser trepitjada pel jugador
// Nomes es desactiva si el jugador la toca per dalt (comprova la normal del contacte)
using System.Collections;
using UnityEngine;
public class DisappearingPlatform : MonoBehaviour
{
    public float delayBeforeOff = 0.5f; // Temps fins que desapareix
    public float timeToRespawn = 2f;    // Temps fins que reapareix
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
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Comprova que el contacte és des de dalt (normal apunta cap avall)
                if (contact.normal.y < -0.5f) { StartCoroutine(DisappearRoutine()); break; }
            }
        }
    }

    IEnumerator DisappearRoutine()
    {
        isDisappearing = true;
        yield return new WaitForSeconds(delayBeforeOff);
        animator.SetBool("isOn", false);
        col.isTrigger = true; // El jugador ja pot travessar la plataforma
        yield return new WaitForSeconds(timeToRespawn);
        col.isTrigger = false;
        animator.SetBool("isOn", true);
        isDisappearing = false;
    }
}