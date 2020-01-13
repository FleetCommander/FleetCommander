using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour {
    private Vector3 _sphereExpandPoint;
    private float _expand;
    
    [SerializeField]
    [Range(10,50)]
    private float _scaleFactor = 1;
    
    [SerializeField ] private Material invisiblemat;

    private Color standardcolor;
    private Material[] material1;
    
    public Stack<Material> StandardCol = new Stack<Material>();
    
    // Start is called before the first frame update
    void Awake() {
        
    }

    // Update is called once per frame
    void Update() {

//        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch) > 0.5f) {
//            //Mache kugel sichtbar
//             //if analogstick
//                //verändere Radius von Kugel
//
//
//            //else
//                // mache Kugel unsichtbar
//        }

         _expand =  OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y * _scaleFactor *  Time.deltaTime;
        
         //TODO Lineare Funktion auswechseln zur quadratischen?
         transform.localScale += new Vector3(_expand, _expand, _expand);

     /*    if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y > 0) {

             transform.localScale =  transform.localScale * _scaleFactor;
         }
     */    
         
         
         
    }
}
