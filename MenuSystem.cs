using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void Play()
    {
        LifeManager.lives = 3;
        RespawnManager.respawnPoints.Clear();
        RespawnManager.lastCheckpointID = "";
        GameManager.ResetFruits(); // reseteja les fruites
        GameManager.ResetEnemies(); // reseteja els enemics
        GameManager.hasShootPower = false;
        TimerManager.currentTime = -1f; // reseteja el temps
        SceneManager.LoadScene(1);
        //Debug.Log("Respawn points count: " + RespawnManager.respawnPoints.Count);
        //Debug.Log("LastCheckpointID: " + RespawnManager.lastCheckpointID);
    }

    public void Exit()
    {
        Application.Quit();
    }
}