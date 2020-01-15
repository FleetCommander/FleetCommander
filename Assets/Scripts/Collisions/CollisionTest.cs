using UnityEngine;

public class CollisionTest : MonoBehaviour {
    
    public GameObject explosionEffect;
    bool hasExploded = false;
    
    void Exploding() {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {

        var plane = gameObject.GetComponent<Renderer>();
        var destroy = other.gameObject.GetComponent<Renderer>();
        
        

            if (plane.sharedMaterial.color.Equals(destroy.sharedMaterial.color)) {
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
