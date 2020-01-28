using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour {
    
    public bool isSelected;

    public bool getSelected() {
        return isSelected;
    }
    // Start is called before the first frame update
    void Start() {
        isSelected = false;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void MySelection() {
        if (isSelected) {
            isSelected = false;
        }
        else {
            isSelected = true;
        }
    }
    
    
}

