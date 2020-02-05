﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    
    private float ufoCount;
    private int passedUfoCount;
    
    [SerializeField] 
    void Awake() {
        CountUfos("Blue Ufos");
        CountUfos("Red Ufos");
        CountUfos("Yellow Ufos");
        Debug.Log("The ufos in this scene are " + ufoCount);
    }

    private void CountUfos(string groupName) {
        GameObject group = GameObject.Find(groupName);
        foreach (Transform child in group.transform) {
            ufoCount++;
        }
    }

    // Update is called once per frame
    void Update() {
        
        if (Get80Percent(ufoCount) <= passedUfoCount) {
            GoToNextScene();
        }

        if (OVRInput.Get(OVRInput.Button.One) && 
            OVRInput.Get(OVRInput.Button.Two) &&
            OVRInput.Get(OVRInput.Button.Three) && 
            OVRInput.Get(OVRInput.Button.Four) &&
            (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) >= 0.9f) &&
            (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= 0.9f)) {

            DataContainer.GetInstance().skippedLevel = true;
            GoToNextScene();
            
        }
    }

    private static void GoToNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private float Get80Percent(float count) {
        return Mathf.Ceil((count / 100) * 80);
    }

    public void AddPassedUfo() {
        passedUfoCount++;
    }

    public void Print() {
        Debug.Log(passedUfoCount);
    }
}
