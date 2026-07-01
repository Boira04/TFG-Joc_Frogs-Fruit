// SceneTransition.cs
// Gestiona la transicio visual entre escenes amb una "cortina" negra
// que sexpandeix desquerra a dreta en sortir i es plega de dreta a esquerra en entrar
// Es un singleton persistent entre escenes (DontDestroyOnLoad)
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
            DontDestroyOnLoad(panel.transform.root.gameObject); // Mante tambe el Canvas
        }
        else Destroy(gameObject);
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
        yield return StartCoroutine(AnimatePanel(0, screenWidth));   // Cortina tancantse
        SceneManager.LoadScene(sceneIndex);
        yield return null;
        yield return StartCoroutine(AnimatePanel(screenWidth, 0));   // Cortina obrintse
    }

    IEnumerator AnimatePanel(float from, float to)
    {
        float elapsed = 0f;
        panel.sizeDelta = new Vector2(from, panel.sizeDelta.y);
        while (elapsed < transitionDuration)
        {
            if (panel == null) yield break;
            elapsed += Time.deltaTime;
            panel.sizeDelta = new Vector2(Mathf.Lerp(from, to, elapsed / transitionDuration), panel.sizeDelta.y);
            yield return null;
        }
        panel.sizeDelta = new Vector2(to, panel.sizeDelta.y);
    }
}