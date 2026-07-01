using UnityEngine;

public class LevelMusicTrigger : MonoBehaviour
{
    public AudioClip levelMusic;

    void Start()
    {
        MusicManager.instance.PlayMusic(levelMusic);
    }
}