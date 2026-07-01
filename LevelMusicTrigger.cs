// LevelMusicTrigger.cs
// Demana al MusicManager que reprodueixi la musica assignada a cada nivell
// Gracies a la comprovacio del MusicManager, la musica no es reinicia si ja estava sonant
using UnityEngine;
public class LevelMusicTrigger : MonoBehaviour
{
    public AudioClip levelMusic;
    void Start() => MusicManager.instance.PlayMusic(levelMusic);
}