// BulletSpawner.cs
// Spawner de BadBullet situat al passadis del nivell final
// Deixa de disparar quan el jugador sapropa massa (stopDistance)
using UnityEngine;
public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 2f;         // Temps entre disparos
    public float stopDistance = 3f;     // Distancia minima per deixar de disparar

    private Transform player;
    private float timer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = fireRate;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < stopDistance) return; // Zona de seguretat: no dispara si el jugador es massa a prop

        timer -= Time.deltaTime;
        if (timer <= 0f) { Shoot(); timer = fireRate; }
    }

    private void Shoot()
    {
        if (bulletPrefab == null) return;
        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        BadBullet badBullet = b.GetComponent<BadBullet>();
        if (badBullet != null) badBullet.Init(player);
    }

    // Visualitzar la zona de seguretat a leditor (cercle verd)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}