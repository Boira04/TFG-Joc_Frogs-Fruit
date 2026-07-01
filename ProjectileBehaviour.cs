// ProjectileBehaviour.cs
// Projectil del jugador que es mou en linia recta (esquerra o dreta)
// Ignora la colisio amb el propi jugador i es destrueix en tocar qualsevol obstacle
using UnityEngine;
public class ProjectileBehaviour : MonoBehaviour
{
    public float Speed = 4.5f;
    public bool goingRight = true;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
    }

    void Update()
    {
        transform.position += (goingRight ? Vector3.right : Vector3.left) * Speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) => Destroy(gameObject);
}