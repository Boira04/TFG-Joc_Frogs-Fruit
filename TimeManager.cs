using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public float timeLimit = 60f;
    public static float currentTime = -1f; // -1 indica que no s'ha inicialitzat
    public Image timerBar;

    void Start()
    {
        // Si és la primera vegada, inicialitza el temps
        if (currentTime < 0)
            currentTime = timeLimit;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        timerBar.fillAmount = currentTime / timeLimit;

        if (currentTime <= 0)
        {
            currentTime = -1f; // reseteja per la propera partida
            LifeManager.lives = 3;
            RespawnManager.respawnPoints.Clear();
            RespawnManager.lastCheckpointID = "";
            GameManager.ResetFruits();
            SceneManager.LoadScene(0);
        }
    }
}