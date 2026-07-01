// MenuSystem.cs
// Gestiona els botons del menu principal: Play reseteja tot lestat del joci carrega el primer nivell; Exit tanca laplicacio
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuSystem : MonoBehaviour
{
    public void Play()
    {
        LifeManager.lives = 3;
        RespawnManager.respawnPoints.Clear();
        RespawnManager.lastCheckpointID = "";
        GameManager.ResetFruits();
        GameManager.ResetEnemies();
        GameManager.hasShootPower = false;
        GameManager.bossHitPoints = -1f;
        TimerManager.currentTime = -1f;
        SceneManager.LoadScene(1);
    }

    public void Exit() => Application.Quit();
}