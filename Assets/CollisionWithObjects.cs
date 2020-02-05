using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWithObjects : MonoBehaviour {
    
    [SerializeField] private GameObject explosionEffect;
    private bool hasExploded;
    private const string HAZARD = "Hazard";
    private const string GOAL = "Goal";

    private void OnTriggerEnter(Collider other) {
        
        
        
        if (other.tag.Equals(GOAL) && other.GetComponent<GoalColor>().goalColor.Equals(GetComponentInParent<UfoColor>().ufoColors)) {
            Destroy(transform.parent.gameObject);
            //todo success play sound
        }
        
        if (other.tag.Equals(HAZARD)) {
            Explode(other.gameObject);
            Explode(transform.parent.gameObject);
        }
        
        else {
            Explode(transform.parent.gameObject);
        }
    }


    private void Explode(GameObject target) {
        DataContainer.getInstance().mistakes++;
        Instantiate(explosionEffect, target.transform.position, target.transform.rotation);
        //Todo wrong feedback
        Destroy(target);
    }
}
