using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class LaserMethod : MonoBehaviour {
    
    [SerializeField] private Modes mode = Modes.SELECTION;
    [SerializeField] [Range(2f, 10f)] private float bobbelSpeed = 2f;
    [SerializeField] private GameObject bobbel;
    [SerializeField] private Material invisiblemat;
    [FormerlySerializedAs("endPosition")] public Vector3 rayCastEndPosition;
    [SerializeField] private ModeChangedEvent modeChanged;
    [SerializeField] private TargetPositionEvent targetPositionEvent;

    private static readonly int Outline = Shader.PropertyToID("_Outline");
    private const string SELECTABLE = "Selectable";
    private Stack<GameObject> lastSelectedStack = new Stack<GameObject>();
    private Stack<Material> standardCol = new Stack<Material>();

    private GameObject go;
    private Material material;
    private float targetLength = 5.0f;
    private LineRenderer lineRenderer;
    private Transform lastHitTransform;
    private Color standardcolor;
    private Material[] material1;
    private float buttonDownTimer;
    private float delay = 1f;
    private Renderer bobbelRenderer;


    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        bobbel.transform.position = transform.position;
        bobbelRenderer = bobbel.GetComponent<Renderer>();
        
    }

    void Update() {
        bobbelRenderer.enabled = (mode == Modes.NAVIGATION);
        bobbel.transform.localScale = CalcScale(bobbel.transform.position - transform.position);

        if (OVRInput.GetDown(OVRInput.Button.Start)) {
            ToggleModeAndInvokeEvent();
        }


        if (OVRInput.GetDown(OVRInput.Button.Two)) {
            DeselectLast();
        }

        if (OVRInput.GetDown(OVRInput.Button.Four)) {
            DeselectAll();
        }
        Laser();
    }

    private Vector3 CalcScale(Vector3 distanceVector) {
        float distance = distanceVector.magnitude;
        return new Vector3(distance, distance, distance) * 0.05f;
    }

    private void ToggleModeAndInvokeEvent() {
        if (mode == Modes.SELECTION) {
            mode = Modes.NAVIGATION;
        }
        else if (mode == Modes.NAVIGATION) {
            mode = Modes.SELECTION;
        }

        modeChanged.Invoke(mode);
    }

    private void Laser() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        

        int raycastLength = 5000;
        rayCastEndPosition = transform.position + (transform.forward * raycastLength);

        if (mode == Modes.SELECTION) {
            if (Physics.Raycast(ray, out hit, raycastLength) && hit.transform.CompareTag(SELECTABLE)) {
                LaserOnSelectable(hit);
            }
            else if (go != null && go.GetComponent<Selected>().isSelected == false) {
                lastHitTransform.GetComponent<Renderer>().material.SetFloat(Outline, 0);
            }
        }

        if (mode == Modes.NAVIGATION) {
            navigationBobble();
        }


        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, rayCastEndPosition);
    }

    private void navigationBobble() {
        
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y > 0.8 ||
            OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y < -0.8) {
            bobbelSpeed += 5f;
        }
        else {
            bobbelSpeed = 2;
        }

        float threshold = (bobbel.transform.position +
                           bobbelSpeed * OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y *
                           (rayCastEndPosition - bobbel.transform.position).normalized - transform.position).magnitude;

        var limit = 2;
        if (threshold >= limit) {
            bobbel.transform.position += bobbelSpeed * OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y *
                                         (rayCastEndPosition - bobbel.transform.position).normalized;
        }
        else {
            bobbel.transform.position =
                transform.position + (rayCastEndPosition - bobbel.transform.position).normalized * limit;
        }

        if (OVRInput.GetDown(OVRInput.Button.One)) {
            targetPositionEvent.Invoke(bobbel.transform.position);
        }
    }

    private void LaserOnSelectable(RaycastHit hit) {
        lastHitTransform = hit.transform;
        hit.transform.GetComponent<Renderer>().material.SetFloat(Outline, 0.2f);
        go = hit.transform.gameObject;
        rayCastEndPosition = hit.point;


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
                //  hit.transform.GetComponent<MeshRenderer>().materials = material1;
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
        targetLength = targetLength + OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y * bobbelSpeed;
        if (targetLength <= 0) {
            targetLength = 0;
        }
    }
}