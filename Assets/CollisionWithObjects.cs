using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            
            FindObjectOfType<SoundManager>().Play("Richtig");
        }
        else if (other.tag.Equals(HAZARD)) {
            Explode(other.gameObject);
            GameObject levelManager = GameObject.Find("LevelManager");
            levelManager.GetComponent<LevelManager>().AddFailedUfos();
            Explode(transform.parent.gameObject);
            
            
        }
        else if(other.tag.Equals(GOAL) && !other.GetComponent<GoalColor>().goalColor.Equals(GetComponentInParent<UfoColor>().ufoColors)){
            GameObject levelManager = GameObject.Find("LevelManager");
            levelManager.GetComponent<LevelManager>().AddFailedUfos();
            Explode(transform.parent.gameObject);
        }
    }


    private void Explode(GameObject target) {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1)) {
            DataContainer.GetInstance().level1Mistakes++;
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(2)) {
            DataContainer.GetInstance().level2Mistakes++;
        }
        
        Instantiate(explosionEffect, target.transform.position, target.transform.rotation);
        
        FindObjectOfType<SoundManager>().Play("Falsch");
        Destroy(target);
    }
}
