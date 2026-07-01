using UnityEngine;
public class ProjectileBehaviour : MonoBehaviour
{
    public float Speed = 4.5f;
    public bool goingRight = true;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
    }
    void Update()
    {
        if (goingRight)
            transform.position += Vector3.right * Speed * Time.deltaTime;
        else
            transform.position += Vector3.left * Speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}