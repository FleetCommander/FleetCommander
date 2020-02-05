﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Boo.Lang;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


[RequireComponent(typeof(LineRenderer))]
public class BubbleMethod : MonoBehaviour {
    [SerializeField] private Modes mode = Modes.SELECTION;
    [SerializeField] [Range(2f, 10f)] private float bobbelSpeed = 2f;
    [SerializeField] private GameObject bobbelNavigation;
    [SerializeField] private GameObject bobbelSelection;
    [SerializeField] private Material invisiblemat;
    public Vector3 rayCastEndPosition;

    private static readonly int Outline = Shader.PropertyToID("_Outline");
    private const string SELECTABLE = "Selectable";
    public Stack<GameObject> lastSelectedStack = new Stack<GameObject>();
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

    private GameObject lastGo = null;
    private GameObject currentGo;

    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        bobbelNavigation.transform.position = transform.position;
        bobbelRenderer = bobbelNavigation.GetComponent<Renderer>();

        bobbelSelection.transform.position = transform.position;
        bobbelSelectionRenderer = bobbelSelection.GetComponent<Renderer>();


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
        }
        else if (mode == Modes.NAVIGATION) {
            mode = Modes.SELECTION;
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
            int countShips = col.testStack.Count;
            for (int i = 0; i < countShips; i++) {

                double lambda = Math.Pow(-1, i) * Math.Ceiling((double) i / 7) * distance;
                if (i == 0) {
                    endPosition = targetPosition;
                }


                if (i <= 1) {
                    distance = 20;
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
                GameObject lastSelected = col.testStack.Pop();
                materials[1] = standardCol.Pop();

                ShipMovement shipMovement = lastSelected.GetComponent<ShipMovement>();
                shipMovement.targethit = true;
                shipMovement.targetPosition = targetPosition;
                shipMovement.endPosition = endPosition;
                lastSelected.GetComponent<Collider>().enabled = true;
                lastSelected.GetComponent<Renderer>().material.SetFloat(Outline, 0);
                lastSelected.transform.GetComponent<Renderer>().materials = materials;
            }

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
        rayCastEndPosition = bobbelSelection.transform.position;
    }

}