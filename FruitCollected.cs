// FruitCollected.cs
// Gestiona la recollida de fruites. Si la fruita té una llum (Light2D),es la SpecialFruit i atorga el poder de disparar al jugador
using UnityEngine;
public class FruitCollected : MonoBehaviour
{
    public string fruitID;          // ID unic per a cada fruita, assignat a linspector
    public AudioClip collectSound;

    void Start()
    {
        if (GameManager.IsFruitCollected(fruitID)) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false; // Evita doble deteccio durant lanimacio de recollida
            GameManager.CollectFruit(fruitID);
            GetComponent<SpriteRenderer>().enabled = false;
            gameObject.transform.GetChild(0).gameObject.SetActive(true); // Activa lanimacio de recollida

            var light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            if (light2D != null) // Nomes la SpecialFruit te Light2D
            {
                light2D.enabled = false;
                PlayerMove player = FindFirstObjectByType<PlayerMove>();
                player.canShoot = true;
                GameManager.hasShootPower = true;
            }

            if (collectSound != null) AudioSource.PlayClipAtPoint(collectSound, transform.position, 10f);
            Destroy(gameObject, 0.5f);
        }
    }
}