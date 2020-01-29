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
        var foo = transform.parent.gameObject;
        Destroy(foo);
    }

    private void OnTriggerEnter(Collider other) {
        var plane = gameObject.GetComponent<Renderer>().materials[1].color;
        var destroy = other.gameObject.GetComponent<Renderer>().materials[0].color;

        if (gameObject.name.Equals(other.gameObject.name)) {
            var positionPlane1 = gameObject.GetComponent<Transform>().position;
            var positionPlane2 = other.gameObject.GetComponent<Transform>().position;
            var mittelpunkt = (positionPlane1 + positionPlane2) / 2;

        }
        else {

            Debug.Log(gameObject.name);
            Debug.Log(other.gameObject.name);

            if (plane.r == destroy.r && plane.b == destroy.b && plane.g == destroy.g) {
                var foo = transform.parent.gameObject;
                Destroy(foo);
            }
            else {
                if (!hasExploded) {
                    Exploding();
                    hasExploded = true;

                }
            }
        }

    }
}
