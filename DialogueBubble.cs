// DialogueBubble.cs
// Escriu text lletra a lletra amb so retro 8bits, gestionant salts de pagina automatics sense tallar paraules 
// En acabar, torna a escena del menu principal
using UnityEngine;
using System.Collections;
using TMPro;
public class DialogueBubble : MonoBehaviour
{
    public TextMeshPro dialogueText;
    public AudioClip letterSound;
    public float timeBetweenLetters = 0.05f;
    public float startDelay = 0f;       // Retard inicial configurable des de linspector
    public int soundEveryXLetters = 2;  // Reprodueix el so cada X lletres per evitar solapaments
    public int maxLines = 3;            // Maxim de linies visibles al bocadillo
    [TextArea] public string fullText;

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
            if (MusicManager.instance != null) MusicManager.instance.StopMusic(); // Atura la musica en començar el dialeg
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

            // Test silenciós: comprova si la paraula cap sense mostrar-la al jugador
            string testText = currentPageText.Length > 0 ? currentPageText + " " + word : word;
            dialogueText.text = testText;
            dialogueText.ForceMeshUpdate();
            int lineCount = dialogueText.textInfo.lineCount;
            dialogueText.text = currentPageText; // Restaura el text visible

            if (lineCount > maxLines)
            {
                yield return new WaitForSeconds(0.8f); // Pausa perque el jugador llegeixi
                currentPageText = "";
                dialogueText.text = "";
            }

            string prefix = currentPageText.Length > 0 ? currentPageText + " " : "";

            // Escriu la paraula lletra a lletra
            for (int i = 1; i <= word.Length; i++)
            {
                dialogueText.text = prefix + word.Substring(0, i);
                letterCount++;
                if (letterSound != null && letterCount % soundEveryXLetters == 0)
                    AudioSource.PlayClipAtPoint(letterSound, transform.position, 0.5f);
                yield return new WaitForSeconds(timeBetweenLetters);
            }

            currentPageText = prefix + word;
        }

        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // Torna al menu principal
    }
}