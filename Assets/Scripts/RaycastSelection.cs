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
    private Transform lastHitTransform;


    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        RenderRaycast();
    }
    
    private void RenderRaycast() {

        float targetLength = defaultLength;
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        Vector3 endPosition = transform.position + (transform.forward * targetLength);
        dot.transform.position = endPosition;

        if (Physics.Raycast(ray, out hit, defaultLength)) {
            lastHitTransform = hit.transform;
            hit.transform.GetComponent<Renderer>().material.color = Color.red;
            endPosition = hit.point;
        }
        else {
            lastHitTransform.GetComponent<Renderer>().material.color = Color.green;
        }
        
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }
    

}
