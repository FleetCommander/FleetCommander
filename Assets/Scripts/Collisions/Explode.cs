using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {
    public GameObject explosionEffect;
    bool hasExploded = false;

    void Exploding() {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(gameObject.name + "Neeeeeeeeeeeeee" + other.gameObject.name);
        if (!hasExploded) {
        Exploding();
        hasExploded = true;
        }
    }
}
