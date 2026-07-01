using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator animator;
    private bool activated = false;
    public string checkpointID;
    public AudioClip checkpointSound; // Nou camp

    void Start()
    {
        animator = GetComponent<Animator>();
        
        if (RespawnManager.lastCheckpointID == checkpointID)
        {
            activated = true;
            animator.Play("FlagOut");
        }
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

            if (checkpointSound != null)
            {
                AudioSource.PlayClipAtPoint(checkpointSound, transform.position);
            }

            if (LifeManager.lives < 3)
            {
                LifeManager.lives++;
            }
        }
    }

    public static void ResetCheckpoint()
    {
        // ja no cal resetejar static activated
    }
}