using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;

    [Header("Transició")]
    public RectTransform panel;
    public float transitionDuration = 1f;
    public float screenWidth = 1920f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(panel.transform.root.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GoToNextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(TransitionRoutine(nextScene));
    }

    IEnumerator TransitionRoutine(int sceneIndex)
    {
        PlayerMove player = FindFirstObjectByType<PlayerMove>();
        if (player != null)
        {
            player.isDead = true;
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            player.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }

        // Cortina tancant-se: esquerra a dreta
        yield return StartCoroutine(AnimatePanel(0, screenWidth));

        SceneManager.LoadScene(sceneIndex);

        yield return null; // Espera un frame perquè l'escena carregui

        // Cortina obrint-se: dreta a esquerra
        yield return StartCoroutine(AnimatePanel(screenWidth, 0));
    }

    IEnumerator AnimatePanel(float from, float to)
    {
        float elapsed = 0f;
        panel.sizeDelta = new Vector2(from, panel.sizeDelta.y);

        while (elapsed < transitionDuration)
        {
            if (panel == null) yield break;

            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;
            panel.sizeDelta = new Vector2(Mathf.Lerp(from, to, t), panel.sizeDelta.y);
            yield return null;
        }

        panel.sizeDelta = new Vector2(to, panel.sizeDelta.y);
    }
}