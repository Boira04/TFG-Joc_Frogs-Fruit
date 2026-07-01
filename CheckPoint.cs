// Checkpoint.cs
// Gestiona els punts de guardada del joc
// En activarse, guarda la posicio de respawn per a lescena actual i suma una vida al jugador
using UnityEngine;
public class Checkpoint : MonoBehaviour
{
    private Animator animator;
    private bool activated = false;
    public string checkpointID;         // ID unic per a cada checkpoint, assignat a linspector
    public AudioClip checkpointSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Si aquest checkpoint ja estava activat en una sessio anterior, mostral directament activat
        if (RespawnManager.lastCheckpointID == checkpointID) { activated = true; animator.Play("FlagOut"); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !activated)
        {
            activated = true;
            int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            RespawnManager.respawnPoints[currentScene] = transform.position;
            RespawnManager.lastCheckpointID = checkpointID;
            animator.SetBool("Activated", true);
            if (checkpointSound != null) AudioSource.PlayClipAtPoint(checkpointSound, transform.position);
            if (LifeManager.lives < 3) LifeManager.lives++; // Bonus de vida en activar el checkpoint
        }
    }

    public static void ResetCheckpoint() { }
}