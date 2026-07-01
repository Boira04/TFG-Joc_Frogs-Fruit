// LifeUI.cs
// Actualitza la UI dels cors mostrant/amagant els hearts segons les vides actuals
using UnityEngine;
public class LifeUI : MonoBehaviour
{
    public GameObject[] hearts;
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].SetActive(i < LifeManager.lives);
    }
}