// MusicManager.cs
// Singleton que gestiona la musica de fons de forma persistent entre escenes
// No reinicia la canço si el clip demanat ja sesta reproduint
using UnityEngine;
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); audioSource = GetComponent<AudioSource>(); }
        else Destroy(gameObject);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return; // Ja sesta reproduint, no la reiniciem
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopMusic() => audioSource.Stop();
}