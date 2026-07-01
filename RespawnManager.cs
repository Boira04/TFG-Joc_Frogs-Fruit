using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public static Dictionary<int, Vector3> respawnPoints = new Dictionary<int, Vector3>();
    public static string lastCheckpointID = "";

    void Start()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        
        if (respawnPoints.ContainsKey(currentScene))
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = respawnPoints[currentScene];
        }
    }
}