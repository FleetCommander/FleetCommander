using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;


public class Navigation : MonoBehaviour {
    private Vector3 _sphereExpandPoint;
    private float _expand;
    public Vector3 targetPosition;
    public bool targethit = false;
    
    [SerializeField]
    [Range(10,50)]
    private float _scaleFactor = 1;
    
    [SerializeField] private Material invisiblemat;
    [SerializeField] private GameObject target;

    private Color standardcolor;
    private Material[] material1;
    
    public Stack<Material> StandardCol = new Stack<Material>();
    
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        targetClick();
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

    void targetClick() {
        if (OVRInput.GetDown(OVRInput.Button.One)) {
            GameObject laser = GameObject.Find("Laser");
            RaycastSelection rayCast = laser.GetComponent<RaycastSelection>();
            targetPosition = rayCast.endPosition;
            targethit = true;
            
            Debug.Log(targethit);
        }
    }
}
