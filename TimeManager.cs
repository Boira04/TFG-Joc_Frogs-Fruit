// TimerManager.cs
// Gestiona un temporitzador de nivell. Si sacaba el temps, reseteja el joc i torna al menu
// currentTime es static per persistir entre escenes sense DontDestroyOnLoad
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TimerManager : MonoBehaviour
{
    public float timeLimit = 60f;
    public static float currentTime = -1f; // -1 = no inicialitzat
    public Image timerBar;

    void Start() { if (currentTime < 0) currentTime = timeLimit; }

    void Update()
    {
        currentTime -= Time.deltaTime;
        timerBar.fillAmount = currentTime / timeLimit;
        if (currentTime <= 0)
        {
            currentTime = -1f;
            LifeManager.lives = 3;
            RespawnManager.respawnPoints.Clear();
            RespawnManager.lastCheckpointID = "";
            GameManager.ResetFruits();
            SceneManager.LoadScene(0);
        }
    }
}