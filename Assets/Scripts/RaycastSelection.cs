using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastSelection : MonoBehaviour {
    private const string SELECTABLE = "Selectable"; // TODO ali, Tags in ENUMS auslagern
    private LineRenderer lineRenderer;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {

        RaycastHit hit;

        lineRenderer.SetPosition(0, transform.position);
        Debug.DrawRay(transform.position, Vector3.forward, Color.red, 0);
        if (Physics.Raycast(transform.position, Vector3.forward, out hit)) {
            if (hit.collider.tag.Equals(SELECTABLE)) {
                lineRenderer.SetPosition(1, new Vector3(0, 0, hit.distance));
            }

        } else {
            lineRenderer.SetPosition(1, transform.forward * 2000);
        }



    }

}
