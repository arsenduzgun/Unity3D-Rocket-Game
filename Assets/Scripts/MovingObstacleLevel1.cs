using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingObstacleLevel1 : MonoBehaviour
{
    Vector3 movementVector = new Vector3(0f, -5f, 0f);
    const float tau = Mathf.PI * 2f;
    const float period = 6f;
    float cycle;
    Vector3 startingPosition;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        cycle = Time.time / 6f;
        float sinWave = Mathf.Sin(cycle * tau);
        float movementUnit = sinWave / 2f + 0.5f;
        Vector3 offset = movementUnit * movementVector;
        transform.position = startingPosition + offset;
    }
}
