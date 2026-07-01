using UnityEngine;
using UnityEngine.UI;
public class LifeUI : MonoBehaviour
{
    public GameObject[] hearts;
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < LifeManager.lives);
        }
    }
}