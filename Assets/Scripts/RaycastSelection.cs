using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSelection : MonoBehaviour{

    float maxDistance = 500;


    void Start(){

    }

    // Update is called once per frame
    void Update(){
        Debug.Log(gameObject.transform.position);
        Debug.DrawRay(gameObject.transform.position, transform.TransformDirection(Vector3.forward), Color.red, 0, true);
        
    }

}
