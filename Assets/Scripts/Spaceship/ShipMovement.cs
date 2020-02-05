using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {
    private float speed = 0.1f;
    private float rotationSpeed = 0.3f;
    private float step;
    public Vector3 targetPosition;
    [SerializeField] private bool isSelected = false;
    [SerializeField] private Selected select;
    [SerializeField] public bool targethit = false;
    private UfoColors ufoColors;
    public Vector3 endPosition;
    
    public void Awake() {
        select = GetComponent<Selected>();
        
    }

    void Update() {
        isSelected = select.isSelected;
        if(targethit)
            move();
    }
   
    public void move() {
        if (transform.parent.position != targetPosition) {
            if ((transform.parent.position - targetPosition).magnitude <= 8) {
                targetPosition = endPosition;
                step -= 0.00005f;
            }
            step += 0.0004f;
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, targetPosition, step);    
        }
        
        else {
            step = 0.1f;
            targetPosition = endPosition;
            if (transform.parent.position == targetPosition) {
                targethit = false;
            }
           
        }
    }
}
