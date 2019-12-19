using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastSelection : MonoBehaviour {
    private const string SELECTABLE = "Selectable";

    [SerializeField] private GameObject dot;

    [SerializeField] [Range(0.05f, 0.3f)] private float rayScalingSpeed = 0.1f;

    private Material material;
    private float targetLength = 5.0f;
    private LineRenderer lineRenderer = null;
    private Transform lastHitTransform;
    public Material invisible;
    public GameObject go;
    public Stack<GameObject> LastSelected = new Stack<GameObject>();

    
    
    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        LaserPointer();
    }

    private void LaserPointer() {
        
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        int raycastLength = 5000;
        Vector3 endPosition = transform.position + (transform.forward * raycastLength);
        dot.transform.position = endPosition;
        
        if(OVRInput.GetDown(OVRInput.Button.Four)) {
            int countShips = LastSelected.Count;
            for (int i = 0; i <= countShips; i++) {
                GameObject gotmp = LastSelected.Pop();
                gotmp.GetComponent<Collider>().enabled = true;
                gotmp.GetComponent<Selected>().MySelection();
                gotmp.GetComponent<Renderer>().material.SetFloat("_Outline", 0);
            }
        }
        
        
        
        if(OVRInput.GetDown(OVRInput.Button.Two)) {

            GameObject gotmp = LastSelected.Pop();
            gotmp.GetComponent<Collider>().enabled = true;
            gotmp.GetComponent<Selected>().MySelection();
            gotmp.GetComponent<Renderer>().material.SetFloat("_Outline", 0);

        }

        if (Physics.Raycast(ray, out hit, raycastLength) && hit.transform.CompareTag(SELECTABLE)) {
            lastHitTransform = hit.transform;
            hit.transform.GetComponent<Renderer>().material.SetFloat("_Outline", 0.2f);
//          hit.transform.GetComponent<Renderer>().materials[1] = invisible;
            
            go = hit.transform.gameObject;
            endPosition = hit.point;
            
            
            

            if (OVRInput.Get(OVRInput.Button.One)) {
                go.GetComponent<Selected>().MySelection();
                go.GetComponent<Collider>().enabled = false;
                hit.transform.GetComponent<Renderer>().materials[1].color = Color.clear;
                LastSelected.Push(go);
                }
        }
        
        else if (go != null && go.GetComponent<Selected>().isSelected == false) {
            lastHitTransform.GetComponent<Renderer>().material.SetFloat("_Outline", 0);
            
        }

        
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    private void BubbleSelect() {
        targetLength = targetLength + OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y * rayScalingSpeed;
        if (targetLength <= 0) {
            targetLength = 0;
        }
    }
}