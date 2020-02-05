using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    
    private static readonly int Outline = Shader.PropertyToID("_Outline");
    private Material[] materials;
    private Stack<Material> standardCol = new Stack<Material>();
    [SerializeField] private Material invisiblemat;
   
    
    
    private void OnTriggerEnter(Collider col) {
        col.gameObject.GetComponent<Renderer>().material.SetFloat(Outline, 0.2f);
     }

    
    private void OnTriggerStay(Collider col) {
        if (OVRInput.Get(OVRInput.Button.One)) {

            col.gameObject.GetComponent<Selected>().ToggleSelection();
            col.gameObject.GetComponent<Collider>().enabled = false;
            transform.parent.GetComponent<BubbleMethod>().materials = col.gameObject.GetComponent<MeshRenderer>().materials;
            Material mat1 = transform.parent.GetComponent<BubbleMethod>().materials[1];
            transform.parent.GetComponent<BubbleMethod>().standardCol.Push(mat1);
            transform.parent.GetComponent<BubbleMethod>().materials[1] = invisiblemat;
            col.gameObject.transform.GetComponent<MeshRenderer>().materials = transform.parent.GetComponent<BubbleMethod>().materials;
            transform.parent.GetComponent<BubbleMethod>().lastSelectedStack.Push(col.gameObject);
        }
    }

    private void OnTriggerExit(Collider col) {
        
        col.gameObject.GetComponent<Renderer>().material.SetFloat(Outline, 0f);
    }
}
