// GameManager.cs
// Gestor estatic global que persiteix entre escenes sense DontDestroyOnLoad
// Guarda lestat de les fruites recollides, els enemics i els poders del jugador
using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    private static HashSet<string> collectedFruits = new HashSet<string>();
    private static Dictionary<string, EnemyState> enemyStates = new Dictionary<string, EnemyState>();
    public static float bossHitPoints = -1f; // -1 = no sha guardat cap estat del boss
    public static bool hasShootPower = false;

    public struct EnemyState { public float hitPoints; public Vector3 position; }

    public static void SaveEnemy(string id, float hp, Vector3 pos) => enemyStates[id] = new EnemyState { hitPoints = hp, position = pos };
    public static EnemyState? GetEnemyState(string id) => enemyStates.ContainsKey(id) ? enemyStates[id] : (EnemyState?)null;
    public static void ResetEnemies() => enemyStates.Clear();
    public static void CollectFruit(string id) => collectedFruits.Add(id);
    public static bool IsFruitCollected(string id) => collectedFruits.Contains(id);
    public static void ResetFruits() => collectedFruits.Clear();
}