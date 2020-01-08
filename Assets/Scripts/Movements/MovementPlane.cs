using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlane : MonoBehaviour
{
    
// Geschwindigkeitsanpassungen schneller, bzw langsamer (per Knopfdruck)

    private float speed = 10f;

    void Update() {
        
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
