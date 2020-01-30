using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour {
    
    public bool isSelected;
    
    // Start is called before the first frame update
    void Start() {
        isSelected = false;
    }
    
    public void ToggleSelection() {
        if (isSelected) {
            isSelected = false;
        }
        else {
            isSelected = true;
        }
    }
}

