using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour {
  
    private void OnTriggerEnter(Collider other) {

        var plane = gameObject.GetComponent<Renderer>();
        var destroy = other.gameObject.GetComponent<Renderer>();
        
        if (plane.sharedMaterial.color == destroy.sharedMaterial.color) {
            Destroy(gameObject);
        }
    }
}
