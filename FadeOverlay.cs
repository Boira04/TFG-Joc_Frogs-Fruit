// FadeOverlay.cs
// Fa un fade de negre a transparent sobre una imatge que tapa l'escena, revelant el contingut que hi ha al darrere (animacio de mi en pixel art)
using UnityEngine;
public class FadeOverlay : MonoBehaviour
{
    public SpriteRenderer blackOverlay;
    public float startDelay = 20f;  // Quan comença el fade (en segons des de linici de lescena)
    public float fadeDuration = 2f; // Durada de la dissolució
    private float timer = 0f;
    private bool fadeFinished = false;

    void Start()
    {
        Color c = blackOverlay.color;
        blackOverlay.color = new Color(c.r, c.g, c.b, 1f); // Comença totalment negre
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
                blackOverlay.color = new Color(c.r, c.g, c.b, 0f);
            }
        }
    }
}