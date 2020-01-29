﻿using System;
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
    
    public void Awake() {
        if (GetComponent<Renderer>().material.name.Equals("RED")) {
            ufoColors = UfoColors.RED;
        }
        select = GetComponent<Selected>();
    }

    void Update() {
        isSelected = @select.isSelected;
        if(targethit && isSelected)
            move();
    }
   
    public void move() {
        if (transform.position != targetPosition) {
            step += 0.0004f;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);    
        }
        else {
            step = 0.1f;
            targethit = false;
            @select.MySelection();
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