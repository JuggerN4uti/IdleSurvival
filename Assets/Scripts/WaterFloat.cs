using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFloat : MonoBehaviour
{
    public float amplitude = 0.1f;   // Jak wysoko ma si� podnosi�
    public float frequency = 1f;     // Jak szybko ma si� porusza�

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPosition + new Vector3(0f, offsetY, 0f);
    }
}

