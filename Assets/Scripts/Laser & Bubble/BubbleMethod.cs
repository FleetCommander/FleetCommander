using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


[RequireComponent(typeof(LineRenderer))]
public class BubbleMethod : MonoBehaviour {
    [SerializeField] private Modes mode = Modes.SELECTION;
    [SerializeField] [Range(2f, 10f)] private float bobbelSpeed = 2f;
    [SerializeField] [Range(2f, 10f)] private float bobbelSpeedSelection = 2f;
    [SerializeField] private GameObject bobbelNavigation;
    [SerializeField] private GameObject bobbelSelection;
    [SerializeField] private Material invisiblemat;
    public Vector3 rayCastEndPosition;

    private static readonly int Outline = Shader.PropertyToID("_Outline");
    private const string SELECTABLE = "Selectable";
    public Stack<GameObject> lastSelectedStack = new Stack<GameObject>();
    public Stack<Material> standardCol = new Stack<Material>();

    private GameObject go;
    private Material material;
    private float targetLength = 5.0f;
    private LineRenderer lineRenderer;
    private Transform lastHitTransform;
    private Color standardcolor;
    public Material[] materials;
    private float buttonDownTimer;
    private float delay = 1f;
    private Renderer bobbelRenderer;
    private Renderer bobbelSelectionRenderer;

    private GameObject lastGo = null;
    private GameObject currentGo;

    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        bobbelNavigation.transform.position = transform.position;
        bobbelRenderer = bobbelNavigation.GetComponent<Renderer>();

        bobbelSelection.transform.position = transform.position;
        bobbelSelectionRenderer = bobbelSelection.GetComponent<Renderer>();

       
        bobbelSelection.transform.position = transform.position + transform.forward *5;

    }

    void Update() {
        bobbelRenderer.enabled = (mode == Modes.NAVIGATION);
        bobbelSelectionRenderer.enabled = (mode == Modes.SELECTION);
        bobbelSelection.GetComponent<Collider>().enabled = (mode == Modes.SELECTION);
        
     
        bobbelNavigation.transform.localScale = CalcScale(bobbelNavigation.transform.position - transform.position);

        if (OVRInput.GetDown(OVRInput.Button.Start)) {
            ToggleMode();
        }

        if (OVRInput.GetDown(OVRInput.Button.Two)) {
            DeselectLast();
        }

        if (OVRInput.GetDown(OVRInput.Button.Four)) {
            DeselectAll();
        }

        Laser();
    }

    private void LateUpdate() {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, rayCastEndPosition);
    }

    private void ToggleMode() {
        if (mode == Modes.SELECTION) {
            mode = Modes.NAVIGATION;
            bobbelSelection.SetActive(false);
        }
        else if (mode == Modes.NAVIGATION) {
            mode = Modes.SELECTION;
            bobbelSelection.SetActive(true);
        }
    }

    private void Laser() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        int raycastLength = 5000;
        rayCastEndPosition = transform.position + (transform.forward * raycastLength);

        if (mode == Modes.SELECTION) {
            bubbleSel();
         }   
        if (mode == Modes.NAVIGATION) {
                navigationBobble();
        }
        
    }

    
    private void navigationBobble()
    {

        OnCollision col = bobbelNavigation.GetComponent<OnCollision>();

        
        //lastSelectedStack = col.testStack;
        
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y > 0.8 ||
            OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y < -0.8) {
            bobbelSpeed += 5f;
        }
        else {
            bobbelSpeed = 2;
        }

        // Damit die Bobble nicht zu nah kommt.
        float threshold = (bobbelNavigation.transform.position +
                           bobbelSpeed * OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y *
                           (rayCastEndPosition - bobbelNavigation.transform.position).normalized - transform.position).magnitude;
        var limit = 2;
        if (threshold >= limit) {
            bobbelNavigation.transform.position += bobbelSpeed * OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y *
                                         (rayCastEndPosition - bobbelNavigation.transform.position).normalized;
        }
        else {
            bobbelNavigation.transform.position =
                transform.position + (rayCastEndPosition - bobbelNavigation.transform.position).normalized * limit;
        }

        // Losschicken zum Punkt
        if (OVRInput.GetDown(OVRInput.Button.One)) {
            
            Vector3 targetPosition = bobbelNavigation.transform.position;
            Vector3 endPosition = targetPosition;
            float distance = 10;
            //int countShips = lastSelectedStack.Count;
            int countShips = lastSelectedStack.Count;
            for (int i = 0; i < countShips; i++)
            {

                if (i > 7){
                    distance = 10;
                }
                double lambda = Math.Pow(-1, i) * Math.Ceiling((double) i / 7) * distance;
                if (i == 0) {
                    endPosition = targetPosition;
                }
                if (i % 7 == 1) {
                    endPosition = targetPosition + new Vector3((float) lambda, 0, 0);
                }

                if (i % 7 == 2) {
                    endPosition = targetPosition + new Vector3(0, (float) lambda, 0);
                }

                if (i % 7 == 3) {
                    endPosition = targetPosition + new Vector3(0, 0, (float) lambda);
                }

                if (i % 7 == 4) {
                    endPosition = targetPosition + new Vector3((float) lambda, (float) lambda, (float) lambda);
                }

                if (i % 7 == 5) {
                    endPosition = targetPosition + new Vector3((float) lambda, (float) lambda, (float) -lambda);
                }

                if (i % 7 == 6) {
                    endPosition = targetPosition + new Vector3((float) -lambda, (float) lambda, (float) lambda);
                }

                if (i % 7 == 0) {
                    endPosition = targetPosition + new Vector3((float) -lambda, (float) lambda, (float) -lambda);
                }


                //GameObject lastSelected = lastSelectedStack.Pop();
                GameObject lastSelected = lastSelectedStack.Pop();
                materials[1] = standardCol.Pop();

                ShipMovement shipMovement = lastSelected.GetComponent<ShipMovement>();
                shipMovement.targethit = true;
                shipMovement.targetPosition = targetPosition;
                shipMovement.endPosition = endPosition;
                lastSelected.GetComponent<Collider>().enabled = true;
                lastSelected.GetComponent<Renderer>().material.SetFloat(Outline, 0);
                lastSelected.transform.GetComponent<Renderer>().materials = materials;
                lastSelected.GetComponent<Selected>().ToggleSelection();
            }

            bobbelSelection.transform.position = transform.position + transform.forward *5;
            bobbelSelection.transform.localScale = new Vector3(0.5f,0.5f,0.5f); 
            ToggleMode();
        }
    }

    private void DeselectLast() {
        if (standardCol.Count != 0) {
            GameObject lastSelected = lastSelectedStack.Pop();
            materials[1] = standardCol.Pop();
            lastSelected.GetComponent<Collider>().enabled = true;
            lastSelected.GetComponent<Selected>().ToggleSelection();
            lastSelected.GetComponent<Renderer>().material.SetFloat(Outline, 0);
            lastSelected.transform.GetComponent<Renderer>().materials = materials;
        }
    }

    private void DeselectAll() {
        int countShips = lastSelectedStack.Count;
        for (int i = 0; i < countShips; i++) {
            DeselectLast();
        }
    }

    private Vector3 CalcScale(Vector3 distanceVector) {
        float distance = distanceVector.magnitude;
        return new Vector3(distance, distance, distance) * 0.05f;
    }

    
    private void BubbleSelect() {
        targetLength = targetLength + OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y * bobbelSpeed;
        if (targetLength <= 0) {
            targetLength = 0;
        }
    }

    public void bubbleSel() {

 //       bobbelSelection.transform.position = transform.position + (rayCastEndPosition - bobbelSelection.transform.position).normalized *5;

        float radius = bobbelSelection.GetComponent<SphereCollider>().radius * bobbelSelection.transform.localScale.x;
            //GetComponent<Renderer>().bounds.extents.magnitude;
        
        if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y > 0.8 ||
            OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y < -0.8) {
            bobbelSpeedSelection += 0.05f;
        }
        else {
            bobbelSpeedSelection = 2f;
        }
        Vector3 newPosition2 = new Vector3(0,0,0); 
       
        newPosition2 = bobbelSelection.transform.position + bobbelSpeedSelection *
                           OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y
                           * transform.forward;

        float limit = radius + 5;
        if ((newPosition2 - transform.position).magnitude >= limit)
        {
            bobbelSelection.transform.position = newPosition2;
        }
        else
        {
            bobbelSelection.transform.position = transform.position +
                                                 transform.forward *
                                                 limit;
            bobbelSpeedSelection = 2f;
        }


        

        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y > 0.5)
        {

            bobbelSelection.transform.localScale += new Vector3(1, 1, 1);
        }
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y < -0.5)
        {
            Vector3 newScale2 = new Vector3(0,0,0);
            newScale2 = bobbelSelection.transform.localScale + new Vector3(-1, -1, -1);
            if (newScale2.x < 3)
            {
                bobbelSelection.transform.localScale = new Vector3(0.5f,0.5f,0.5f);                
            }
            else
            {
                bobbelSelection.transform.localScale += new Vector3(-1, -1, -1);    
            }
            
        }
        
        rayCastEndPosition = bobbelSelection.transform.position;
    }

}
