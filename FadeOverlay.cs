using UnityEngine;

public class FadeOverlay : MonoBehaviour
{
    public SpriteRenderer blackOverlay;
    public float startDelay = 20f; // Segons abans de començar el fade
    public float fadeDuration = 2f; // Durada del fade

    private float timer = 0f;
    private bool fadeFinished = false;

    void Start()
    {
        // Assegura't que comença totalment negre
        Color c = blackOverlay.color;
        blackOverlay.color = new Color(c.r, c.g, c.b, 1f);
    }

    void Update()
    {
        if (fadeFinished) return;

        timer += Time.deltaTime;

        if (timer >= startDelay)
        {
            float fadeElapsed = timer - startDelay;
            float alpha = Mathf.Lerp(1f, 0f, fadeElapsed / fadeDuration);

            Color c = blackOverlay.color;
            blackOverlay.color = new Color(c.r, c.g, c.b, alpha);

            if (fadeElapsed >= fadeDuration)
            {
                fadeFinished = true;
                Color finalColor = blackOverlay.color;
                blackOverlay.color = new Color(finalColor.r, finalColor.g, finalColor.b, 0f);
            }
        }
    }
}