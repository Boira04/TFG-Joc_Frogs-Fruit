using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelCompleted : MonoBehaviour
{
    private void Update()
    {
        AllFruitCollected();
    }
    public void AllFruitCollected()
    {
        if(transform.childCount == 0)
        {
            //RespawnManager.respawnPoint = Vector3.zero;
            Checkpoint.ResetCheckpoint();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}