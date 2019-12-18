using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.Log(gameObject.name + "Neeeeeee" + other.gameObject.name);
        
        Destroy(gameObject);
    }
}
