using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying : MonoBehaviour {
    [SerializeField][Range(1,10)]
    private float playerSpeed; // Geschwindigkeit anpassbar
    
    private float turnAround;
    private bool invert = false;
    
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (invert == true) {
            // Thumbstick Taste gedrückt halten um zu nach vorner/hinten zu fliegen (seitwärts geht nicht).
            transform.position = transform.position + Camera.main.transform.forward *
                                 (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y * playerSpeed) * Time.deltaTime;
            transform.position = transform.position +
                                 new Vector3(
                                     -OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x * playerSpeed * Time.deltaTime,
                                     0, 0);
        } 
        else {
            transform.position = transform.position + Camera.main.transform.forward *
                                 (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y * playerSpeed) * Time.deltaTime;
            transform.position = transform.position +
                                 new Vector3(
                                     OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x * playerSpeed * Time.deltaTime,
                                     0, 0);
        }
        
        if (Input.GetButtonDown("Oculus_CrossPlatform_PrimaryThumbstick")) {
            // B taste für direkte 180 Grad Drehung
            turnAround += 180;
            transform.rotation = Quaternion.Euler(0, turnAround, 0);
            invert = !invert;
            //Button Klick soll nie mehr als ein Klick wahrgenommen werden, bis zum loslassen und wieder drücken
        }

    }

}