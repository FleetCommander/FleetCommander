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
            GameObject levelManager = GameObject.Find("LevelManager");
            levelManager.GetComponent<LevelManager>().AddPassedUfo();
            Destroy(transform.parent.gameObject);
            //todo success play sound
        }
        else if (other.tag.Equals(HAZARD)) {
            Explode(other.gameObject);
            Explode(transform.parent.gameObject);
        }
        else if(other.tag.Equals(GOAL) && !other.GetComponent<GoalColor>().goalColor.Equals(GetComponentInParent<UfoColor>().ufoColors)){
            Explode(transform.parent.gameObject);
        }
    }


    private void Explode(GameObject target) {
        DataContainer.GetInstance().mistakes++;
        Instantiate(explosionEffect, target.transform.position, target.transform.rotation);
        //Todo wrong feedback
        Destroy(target);
    }
}
