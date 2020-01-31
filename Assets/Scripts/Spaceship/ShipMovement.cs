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
        if(targethit && isSelected)
            move();
    }
   
    public void move() {
        if (transform.position != targetPosition) {
            if ((transform.position - targetPosition).magnitude <= 8) {
                targetPosition = endPosition;
                step -= 0.00005f;
            }
            step += 0.0004f;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);    
        }

       
        else {
            step = 0.1f;
            targetPosition = endPosition;
            if (transform.position == targetPosition) {
                targethit = false;
                select.ToggleSelection(); 
            }
           
        }
    }
}





/*
Movement für smoothen Raumschiffmovement
public void Movement() {
    transform.position += transform.forward * Time.deltaTime * speed;
    var targetrotation = Quaternion.LookRotation(targetPosition - transform.position);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, rotationSpeed * Time.deltaTime);       
    if (transform.position == targetPosition) {
        speed = 0;
        rotationSpeed = 0;
    }
}*/

/* Bitte nicht löschen Gruß Dennis ^^
Funktion um an anderes Skript zuzugreifen.
GameObject laser = GameObject.Find("Laser");
LaserMethod laserMethod = laser.GetComponent<LaserMethod>();
targethit = laserMethod.targethit;
*/