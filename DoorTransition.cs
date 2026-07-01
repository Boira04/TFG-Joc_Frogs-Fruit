using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneTransition.instance.GoToNextScene();
        }
    }
}