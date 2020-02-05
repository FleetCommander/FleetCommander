﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class UfoColor : MonoBehaviour {
    // Start is called before the first frame update

    [SerializeField] private UfoColors ufoColors;
    private Color color;
    
    // Update is called once per frame
    // Farbfestlegung der Oberflächenfarbe
    void Update() {
        switch (ufoColors) {
            case UfoColors.RED:
                GetComponentInChildren<Renderer>().material.color = Color.red;
                break;
            case UfoColors.YELLOW:
                GetComponentInChildren<Renderer>().material.color = Color.yellow;
                break;
            case UfoColors.BLUE:
                GetComponentInChildren<Renderer>().material.color = Color.blue;
                break;
        }
    }
}