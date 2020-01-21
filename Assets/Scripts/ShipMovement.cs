using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {
    private float speed = 10f;
    private float rotationSpeed = 0.3f;
    private Vector3 targetPosition;
    [SerializeField] private bool isSelected = false;
    [SerializeField] private Selected select;
    [SerializeField] private bool targethit = false;

    public void Awake() {
        select = GetComponent<Selected>();
    }
    
    void Update() {
        GameObject naviObject = GameObject.Find("Navigation Sphere");
        Navigation navi = naviObject.GetComponent<Navigation>();
        targetPosition = navi.targetPosition;
        targethit = navi.targethit;
        isSelected = @select.isSelected;
        if(targethit && isSelected)
            Movement();
    }

    public void Movement() {
        transform.position += transform.forward * Time.deltaTime * speed;
        var targetrotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, rotationSpeed * Time.deltaTime);
        if (transform.position == targetPosition) {
            speed = 0;
            rotationSpeed = 0;
        }
    }
}
