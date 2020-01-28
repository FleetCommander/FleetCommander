using UnityEngine;

[RequireComponent(typeof(CollisionTest), typeof(Collider))]

public class CollisionTest : MonoBehaviour {

    public GameObject explosionEffect;
    bool hasExploded = false;

    private Vector3 _startPosition;

    private void Awake() {
        _startPosition = transform.position;
    }

    void Exploding() {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        var plane = gameObject.GetComponent<Renderer>().materials[1].color;
        var destroy = other.gameObject.GetComponent<Renderer>().materials[0].color;
        
        if (plane.r == destroy.r && plane.b == destroy.b && plane.g == destroy.g) {
            Destroy(gameObject);
        }
        else {
            //Score -1 und explosion
            if (!hasExploded) {
                Exploding();
                hasExploded = true;

            }


        }
    }
}
