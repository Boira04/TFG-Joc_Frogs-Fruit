// CreditsScroll.cs
// Desplaça verticalment el text dels credits cap amunt, amb un retard inicial configurable
using UnityEngine;
public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 50f;
    public RectTransform creditsText;
    public float startDelay = 5.5f;     // Espera que acabi lanimacio del titol
    public float creditsDuration = 15f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= startDelay)
            creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
    }
}