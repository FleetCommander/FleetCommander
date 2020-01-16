using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour {

    public void ToggleVisibility(Modes mode) {
        if (mode == Modes.SELECTION) {
            gameObject.SetActive(false);
        }

        if (mode == Modes.NAVIGATION) {
            gameObject.SetActive(true);
        }
    }

    
}
