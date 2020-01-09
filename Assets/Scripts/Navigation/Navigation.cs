using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour {
    private Vector3 _sphereExpandPoint;
    private float _radius;

    // Start is called before the first frame update
    void Awake() {
    }

    // Update is called once per frame
    void Update() {

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch) > 0.5f) {
            //Mache kugel sichtbar
             //if analogstick
                //verändere Radius von Kugel


            //else
                // mache Kugel unsichtbar
        }
    }
}
