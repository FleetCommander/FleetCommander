using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class LaserMethod : MonoBehaviour {
    
    [SerializeField] private Modes mode = Modes.SELECTION;
    [SerializeField] [Range(2f, 10f)] private float bobbelSpeed = 2f;
    [SerializeField] private GameObject bobbel;
    [SerializeField] private GameObject bobbelSelection;
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
    private Material[] materials;
    private float buttonDownTimer;
    private float delay = 1f;
    private Renderer bobbelRenderer;
    private Renderer bobbelSelectionRenderer;
    

    private Color firstColor = Color.black;
    //Extern Color Schwarz definieren
    // In Laser Abfrage ob Farbe schwarz -> Selectable
    // Nach erster Auswahl -> Setze firstColor auf ausgewählten Wert
    // Bei Auswahl immer Abfrage, ob ausgewähltes Objekt gleich firstColor
    // Stack nach Deselect -> firstColor wieder auf schwarz setzen
    

    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        bobbel.transform.position = transform.position;
        bobbelRenderer = bobbel.GetComponent<Renderer>();
        bobbelSelectionRenderer = bobbelSelection.GetComponent<Renderer>();

    }

    void Update() {
        bobbelRenderer.enabled = (mode == Modes.NAVIGATION);
        bobbel.transform.localScale = CalcScale(bobbel.transform.position - transform.position);

        bobbelSelectionRenderer.enabled = (mode == Modes.BUBBLESELECTION);
        //bobbelSelection.transform.localScale = CalcScale(bobbelSelection.transform.position - transform.position);

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
            mode = Modes.BUBBLESELECTION;
        }
        else if (mode == Modes.BUBBLESELECTION) {
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

        if (mode == Modes.BUBBLESELECTION) {
            bubbleSel();
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
            //DeselectLast();
            int countShips = lastSelectedStack.Count;
            for (int i = 0; i <= countShips; i++) {
                if (lastSelectedStack.Count != 0) {
                    GameObject lastSelected = lastSelectedStack.Pop();
                    Vector3 targetPosition = bobbel.transform.position;
                    ShipMovement shipMethod = lastSelected.GetComponent<ShipMovement>();
                    shipMethod.targethit = true;
                    shipMethod.targetPosition = targetPosition;
                    materials[1] = standardCol.Pop();
                    lastSelected.GetComponent<Collider>().enabled = true;
                    lastSelected.GetComponent<Renderer>().material.SetFloat(Outline, 0);
                    lastSelected.transform.GetComponent<Renderer>().materials = materials;
                    
                }
            }
            //targetPositionEvent.Invoke(bobbel.transform.position);
        }
        
    }

    private void LaserOnSelectable(RaycastHit hit) {
        lastHitTransform = hit.transform;
        hit.transform.GetComponent<Renderer>().material.SetFloat(Outline, 0.2f);
        go = hit.transform.gameObject;
        rayCastEndPosition = hit.point;

        


        if (OVRInput.Get(OVRInput.Button.One)) {
            
            if (buttonDownTimer == 0 || buttonDownTimer > delay) {
                go.GetComponent<Selected>().MySelection();
                go.GetComponent<Collider>().enabled = false;
                materials = hit.transform.GetComponent<MeshRenderer>().materials;
                Material mat1 = materials[1];
                standardCol.Push(mat1);
                materials[1] = invisiblemat;
                hit.transform.GetComponent<MeshRenderer>().materials = materials;
                lastSelectedStack.Push(go);
            }
            buttonDownTimer += Time.deltaTime;

        }
        else {
            buttonDownTimer = 0;
        }
    }

    private void DeselectLast() {
        if (standardCol.Count != 0) {
            GameObject lastSelected = lastSelectedStack.Pop();
            materials[1] = standardCol.Pop();
            lastSelected.GetComponent<Collider>().enabled = true;
            lastSelected.GetComponent<Selected>().MySelection();
            lastSelected.GetComponent<Renderer>().material.SetFloat(Outline, 0);
            lastSelected.transform.GetComponent<Renderer>().materials = materials;
        }
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

    public void bubbleSel() {
        
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y > 0.8 ||
            OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y < -0.8) {
            bobbelSpeed += 5f;
        }
        else {
            bobbelSpeed = 2f;
        }

        float threshold = (bobbelSelection.transform.position +
                           bobbelSpeed * OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y *
                           (rayCastEndPosition - bobbelSelection.transform.position).normalized - transform.position).magnitude;


        float threshold2 = (bobbelSelection.transform.localScale +
                           new Vector3(1, 1, 1 * OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y)).x;
            
            
        var limit = 2;
        if (threshold >= limit) {
            bobbelSelection.transform.position += bobbelSpeed * OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y *
                                         (rayCastEndPosition - bobbelSelection.transform.position).normalized;
        }
        else {
            bobbelSelection.transform.position =
                transform.position + (rayCastEndPosition - bobbelSelection.transform.position).normalized * limit;
        }

        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y > 0.5) {

            if (threshold2 < (rayCastEndPosition - bobbelSelection.transform.position).magnitude) {
                bobbelSelection.transform.localScale += new Vector3(1, 1, 1 * OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y) ;
            }
            

        }
        
        

       
        
        
    }
}