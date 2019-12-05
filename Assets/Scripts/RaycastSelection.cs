using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastSelection : MonoBehaviour {

    private const string SELECTABLE = "Selectable"; // TODO ali, Tags in ENUMS auslagern

    [SerializeField]
    private GameObject dot;

	[SerializeField][Range(0.05f,0.3f)]
	private float rayScaling = 0.1f;

    private Material material;
    private float targetLength = 5.0f;
    private LineRenderer lineRenderer = null;
    private Transform lastHitTransform;


    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        LaserPointer();
    }
    
    private void LaserPointer() {
       

		targetLength = targetLength + OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y * rayScaling ;

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        Vector3 endPosition = transform.position + (transform.forward * targetLength);
        dot.transform.position = endPosition;

        if (Physics.Raycast(ray, out hit, targetLength) && hit.transform.tag == SELECTABLE) {
			
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
