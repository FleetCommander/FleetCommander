using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastSelection : MonoBehaviour {
    private const string SELECTABLE = "Selectable"; // TODO ali, Tags in ENUMS auslagern
    private LineRenderer lineRenderer;
    private Vector3[] points = new Vector3[2];


    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update() {


        points[0] = transform.position;
        points[1] = transform.forward * 1000;
        
        lineRenderer.SetPositions(points);
        Debug.DrawRay(transform.position, transform.forward * 1000, Color.red, 0);


      
    }

}
