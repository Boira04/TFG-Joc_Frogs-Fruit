// LevelCompleted.cs
// Comprova si totes les fruites dun nivell han estat recollides (childCount == 0) i carrega automaticament la seguent escena
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelCompleted : MonoBehaviour
{
    private void Update() => AllFruitCollected();

    public void AllFruitCollected()
    {
        if (transform.childCount == 0)
        {
            Checkpoint.ResetCheckpoint();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}