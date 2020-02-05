using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterGoal : MonoBehaviour {
    
    [SerializeField] private GameObject explosionEffect;
    private bool hasExploded;
    private void OnTriggerEnter(Collider other) {
        
        if (other.GetComponent<GoalColor>().goalColor.Equals(GetComponentInParent<UfoColor>().ufoColors)) {
            Destroy(transform.parent.gameObject);
            //todo success play sound
        }
        else {
            Instantiate(explosionEffect, transform.position, transform.rotation);
            //Todo wrong portal feedback
            Destroy(transform.parent.gameObject);
        }
    }
}
