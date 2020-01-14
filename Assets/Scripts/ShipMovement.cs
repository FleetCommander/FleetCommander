﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {
    private float speed = 10f;
    private float rotationSpeed = 0.3f;
    private Vector3 target;
    [SerializeField] private bool targethit = false;
    

    // Update is called once per frame
    void Update() {
        GameObject naviObject = GameObject.Find("Navigation Sphere");
        Navigation navi = naviObject.GetComponent<Navigation>();
        target = navi.targetPosition;
        targethit = navi.targethit; 
        if(targethit)
            Movement();
    }

    public void Movement() {
        transform.position += transform.forward * Time.deltaTime * speed;
        var targetrotation = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, rotationSpeed * Time.deltaTime);
        if (transform.position == target) {
            speed = 0;
            rotationSpeed = 0;
        }
    }
}
