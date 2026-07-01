using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 2f;
    public float stopDistance = 3f;

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
        if (distance < stopDistance) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Shoot();
            timer = fireRate;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null) return;

        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        BadBullet badBullet = b.GetComponent<BadBullet>();

        if (badBullet != null)
        {
            badBullet.Init(player);
            Debug.Log("Init cridat correctament");
        }
        else
        {
            Debug.LogError("No troba el component BadBullet al prefab!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}