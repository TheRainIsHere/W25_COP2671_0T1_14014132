using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float topBound = 30;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy an out of bounds object
        if (transform.position.z > topBound)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < -topBound)
        {
            Debug.Log("Game Over");     // Game over if an animal reaches the bottom
            Destroy(gameObject);
        }
    }
}
