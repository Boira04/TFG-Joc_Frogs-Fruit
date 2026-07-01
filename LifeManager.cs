// LifeManager.cs
// Gestiona el nombre de vides del jugador de forma estatica entre escenes
using UnityEngine;
public class LifeManager : MonoBehaviour
{
    public static int lives = 3;
    public static void LoseLife() { lives = Mathf.Max(0, lives - 1); }
}