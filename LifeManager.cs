using UnityEngine;
public class LifeManager : MonoBehaviour
{
    public static int lives = 3;
    public static void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            lives = 0;
            // Game Over
        }
    }
}