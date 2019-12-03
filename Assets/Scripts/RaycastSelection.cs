using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastSelection : MonoBehaviour {

    private const string SELECTABLE = "Selectable"; // TODO ali, Tags in ENUMS auslagern

    [SerializeField]
    private GameObject dot;

    private Material material;
    
    private float defaultLength = 5.0f;

    private LineRenderer lineRenderer = null;


    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        RenderRaycast();
    }
    

    private void RenderRaycast() {

        float targetLength = defaultLength;
        RaycastHit hit = CreateRaycast(targetLength);

        Vector3 endPosition = transform.position + (transform.forward * targetLength);
        dot.transform.position = endPosition;

        
        if (hit.collider != null) {
            endPosition = hit.point;

        }
        
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    private RaycastHit CreateRaycast(float length) {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, defaultLength);
        return hit;
    }

}
