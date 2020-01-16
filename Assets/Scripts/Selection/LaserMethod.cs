using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class LaserMethod : MonoBehaviour {

    public Stack<GameObject> lastSelectedStack = new Stack<GameObject>();
    public Stack<Material> standardCol = new Stack<Material>();
    public Vector3 endPosition;


    [SerializeField] private Modes mode = Modes.SELECTION;
    [SerializeField] [Range(0.05f, 0.3f)] private float rayScalingSpeed = 0.1f;
    [SerializeField] private Material invisiblemat;
    [SerializeField] private ModeChangedEvent modeChanged;


    private static readonly int Outline = Shader.PropertyToID("_Outline");
    private const string SELECTABLE = "Selectable";

    private GameObject go;
    private Material material;
    private float targetLength = 5.0f;
    private LineRenderer lineRenderer;
    private Transform lastHitTransform;
    private Color standardcolor;
    private Material[] material1;
    private float buttonDownTimer;
    private float delay = 1f;


    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {

        if (OVRInput.GetDown(OVRInput.Button.Start)) {
            ToggleModeAndInvokeEvent();
        }
        
        
        if(OVRInput.GetDown(OVRInput.Button.Two)) {
            DeselectLast();
        }
        
        if(OVRInput.GetDown(OVRInput.Button.Four)) {
            DeselectAll();
        }
        
        Laser();
    }

    private void ToggleModeAndInvokeEvent() {
        if (mode == Modes.SELECTION) {
            mode = Modes.NAVIGATION;
        }
        else {
            mode = Modes.SELECTION;
        }

        modeChanged.Invoke(mode);
    }

    private void Laser() {
        
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        int raycastLength = 5000;
        endPosition = transform.position + (transform.forward * raycastLength);

        
        
        
        
        
        if (Physics.Raycast(ray, out hit, raycastLength) && hit.transform.CompareTag(SELECTABLE)) {
            if (mode == Modes.SELECTION) {
                LaserOnSelectable(hit);
            }
        }
        else if (go != null && go.GetComponent<Selected>().isSelected == false) {
            lastHitTransform.GetComponent<Renderer>().material.SetFloat(Outline, 0);
        }
        
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    private void LaserOnSelectable(RaycastHit hit) {
        lastHitTransform = hit.transform;
        hit.transform.GetComponent<Renderer>().material.SetFloat(Outline, 0.2f);
        go = hit.transform.gameObject;
        endPosition = hit.point;


        if (OVRInput.GetDown(OVRInput.Button.One)) {
            go.GetComponent<Selected>().MySelection();
            go.GetComponent<Collider>().enabled = false;
            material1 = hit.transform.GetComponent<MeshRenderer>().materials;
            Material mat1 = material1[1];
            standardCol.Push(mat1);
            material1[1] = invisiblemat;
            hit.transform.GetComponent<MeshRenderer>().materials = material1;
            //standardcolor = hit.transform.GetComponent<Renderer>().materials[1].color;
            //hit.transform.GetComponent<Renderer>().materials[1].color = Color.clear;
            lastSelectedStack.Push(go);
        }

        if (OVRInput.Get(OVRInput.Button.One)) {
            buttonDownTimer += Time.deltaTime;
            if (buttonDownTimer > delay) {
                go.GetComponent<Selected>().MySelection();
                go.GetComponent<Collider>().enabled = false;
                material1 = hit.transform.GetComponent<MeshRenderer>().materials;
                Material mat1 = material1[1];
                standardCol.Push(mat1);
                material1[1] = invisiblemat;
                hit.transform.GetComponent<MeshRenderer>().materials = material1;
                lastSelectedStack.Push(go);
            }
        }
    }

    private void DeselectLast() {
        GameObject lastSelected = lastSelectedStack.Pop();
        material1[1] = standardCol.Pop();
        lastSelected.GetComponent<Collider>().enabled = true;
        lastSelected.GetComponent<Selected>().MySelection();
        lastSelected.GetComponent<Renderer>().material.SetFloat(Outline, 0);
        lastSelected.transform.GetComponent<Renderer>().materials = material1;
    }

    private void DeselectAll() {
        int countShips = lastSelectedStack.Count;
        for (int i = 0; i <= countShips; i++) {
            DeselectLast();
        }
    }

    private void BubbleSelect() {
        targetLength = targetLength + OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y * rayScalingSpeed;
        if (targetLength <= 0) {
            targetLength = 0;
        }
    }
}