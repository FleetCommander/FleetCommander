using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour {

    public void ToggleVisibility(Modes mode) {
        if (mode == Modes.SELECTION) {
             gameObject.GetComponent<Renderer>().enabled = false;
        }

        if (mode == Modes.NAVIGATION) {
            gameObject.GetComponent<Renderer>().enabled = true;
        }
    }

    
}
