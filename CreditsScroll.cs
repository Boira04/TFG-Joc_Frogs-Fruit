using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 50f;
    public RectTransform creditsText;
    public float startDelay = 5.5f; // 5:30 = 5.5 segons

    private float timer = 0f;
    public float creditsDuration = 15f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= startDelay)
        {
            creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }
}