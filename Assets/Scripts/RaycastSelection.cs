using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastSelection : MonoBehaviour {
    private const string SELECTABLE = "Selectable"; // TODO ali, Tags in ENUMS auslagern

    [SerializeField] private GameObject dot;

    [SerializeField] [Range(0.05f, 0.3f)] private float rayScalingSpeed = 0.1f;

    private Material material;
    private float targetLength = 5.0f;
    private LineRenderer lineRenderer = null;
    private Transform lastHitTransform;
    public Material invisible;
    public GameObject go;
 
    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        LaserPointer();
    }

    private void LaserPointer() {
        targetLength = targetLength + OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y * rayScalingSpeed;
        if (targetLength <= 0) {
            targetLength = 0;
        }

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        Vector3 endPosition = transform.position + (transform.forward * targetLength);
        dot.transform.position = endPosition;


        if (Physics.Raycast(ray, out hit, targetLength) && hit.transform.CompareTag(SELECTABLE)) {
            lastHitTransform = hit.transform;
            hit.transform.GetComponent<Renderer>().material.SetFloat("_Outline", 0.2f);
//          hit.transform.GetComponent<Renderer>().materials[1] = invisible;

            go = hit.transform.gameObject;
            endPosition = hit.point;

            if (OVRInput.GetDown(OVRInput.Button.One)) {
                go.GetComponent<Selected>().MySelection();
                hit.transform.GetComponent<Renderer>().materials[1].color = Color.clear;
            }
        }
        
        else if (go != null && go.GetComponent<Selected>().isSelected == false) {
            lastHitTransform.GetComponent<Renderer>().material.SetFloat("_Outline", 0);
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }
}