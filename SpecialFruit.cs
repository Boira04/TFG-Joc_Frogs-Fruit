// SpecialFruit.cs
// Fa flotar la fruita especial amunt i avall amb un efecte sinusoidal
// La logica de recollida i poder de disparar es a FruitCollected.cs
using UnityEngine;
public class SpecialFruit : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float floatHeight = 0.3f;
    private Vector3 startPosition;

    void Start() => startPosition = transform.position;

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}