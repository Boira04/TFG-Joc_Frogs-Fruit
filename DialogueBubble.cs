using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueBubble : MonoBehaviour
{
    public TextMeshPro dialogueText;
    public AudioClip letterSound;
    public float timeBetweenLetters = 0.05f;
    public float startDelay = 0f;
    public int soundEveryXLetters = 2;
    public int maxLines = 3;
    [TextArea]
    public string fullText;

    private float timer = 0f;
    private bool started = false;
    private int letterCount = 0;

    void Update()
    {
        if (started) return;

        timer += Time.deltaTime;

        if (timer >= startDelay)
        {
            started = true;
            dialogueText.text = "";

            if (MusicManager.instance != null)
                MusicManager.instance.StopMusic();

            StartCoroutine(TypeText());
        }
    }

    IEnumerator TypeText()
    {
        string[] words = fullText.Split(' ');
        string currentPageText = "";

        for (int w = 0; w < words.Length; w++)
        {
            string word = words[w];

            // Test silenciós
            string testText = currentPageText.Length > 0 ? currentPageText + " " + word : word;
            dialogueText.text = testText;
            dialogueText.ForceMeshUpdate();
            int lineCount = dialogueText.textInfo.lineCount;

            // Restaura el text visible
            dialogueText.text = currentPageText;

            if (lineCount > maxLines)
            {
                // Nova pàgina
                yield return new WaitForSeconds(0.8f);
                currentPageText = "";
                dialogueText.text = "";
            }

            // Escriu la paraula lletra a lletra
            string prefix = currentPageText.Length > 0 ? currentPageText + " " : "";

            for (int i = 1; i <= word.Length; i++)
            {
                dialogueText.text = prefix + word.Substring(0, i);
                letterCount++;

                if (letterSound != null && letterCount % soundEveryXLetters == 0)
                    AudioSource.PlayClipAtPoint(letterSound, transform.position, 0.5f);

                yield return new WaitForSeconds(timeBetweenLetters);
            }

            // Actualitza el text de la pàgina actual
            currentPageText = prefix + word;
        }

        // Quan acaba tot el text, espera i torna al menú
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}