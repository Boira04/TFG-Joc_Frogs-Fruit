using UnityEngine;

public class FruitCollected : MonoBehaviour
{
    public string fruitID;
    public AudioClip collectSound;

    void Start()
    {
        if (GameManager.IsFruitCollected(fruitID))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false; // desactiva immediatament el collider

            GameManager.CollectFruit(fruitID);
            GetComponent<SpriteRenderer>().enabled = false;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);

            var light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

            if (light2D != null)
            {
                light2D.enabled = false;
                PlayerMove player = FindFirstObjectByType<PlayerMove>();
                player.canShoot = true;
                GameManager.hasShootPower = true;
            }

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position, 10f); // el doble de volum
            }

            Destroy(gameObject, 0.5f);
        }
    }
}