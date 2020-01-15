using UnityEngine;

[RequireComponent(typeof(CollisionTest), typeof(Collider))]

public class CollisionTest : MonoBehaviour {
    
    public GameObject explosionEffect;
    bool hasExploded = false;
    
    [SerializeField] private IntVariable _score;
    private Vector3 _startPosition;
    
    private void Start()
    {
        _startPosition = transform.position;
        _score.Value = 0;
    }
    
    void Exploding() {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {

        var plane = gameObject.GetComponent<Renderer>();
        var destroy = other.gameObject.GetComponent<Renderer>();
        
        

            if (plane.sharedMaterial.color.Equals(destroy.sharedMaterial.color)) {
                Destroy(gameObject);
                _score.Value++;
            }
            else {
                //Score -1 und explosion
                if (!hasExploded) {
                    Exploding();
                    hasExploded = true;
                    
                    if(_score.Value > 0) {
                        _score.Value--;
                    }
            }

    }
}
}
