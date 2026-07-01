// DoorTransition.cs
// Trigger invisible que activa la transició a la següent escena quan el jugador hi entra
using UnityEngine;
public class DoorTransition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            SceneTransition.instance.GoToNextScene();
    }
}