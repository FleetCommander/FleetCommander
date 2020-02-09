using System.Collections;
using System.Collections.Generic;
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

        if(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y > 0.8 || OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y < -0.8){
          _scaleFactor += 5;
        }
        else{
          _scaleFactor = 2;
        }

         _expand =  OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y * _scaleFactor *  Time.deltaTime;

        float tmp = transform.localScale.x + _expand;
         if(tmp >= 5){
           transform.localScale += new Vector3(_expand, _expand, _expand);
         }
         else{
           transform.localScale = new Vector3(5, 5, 5);
         }




     /*    if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y > 0) {

             transform.localScale =  transform.localScale * _scaleFactor;
         }
     */



    }

    void targetClick() {
        if (OVRInput.GetDown(OVRInput.Button.One)) {
            GameObject laser = GameObject.Find("Laser");
            LaserMethod laserMethod = laser.GetComponent<LaserMethod>();
            targetPosition = laserMethod.rayCastEndPosition;
            targethit = true;


            Debug.Log(targethit);
        }
    }
}
