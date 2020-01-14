using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastSelection : MonoBehaviour {
    private const string SELECTABLE = "Selectable";

    

    [SerializeField] [Range(0.05f, 0.3f)] private float rayScalingSpeed = 0.1f;
    
    [SerializeField ] private Material invisiblemat;
    
    private Material material;
    private float targetLength = 5.0f;
    private LineRenderer lineRenderer = null;
    private Transform lastHitTransform;
    private Color standardcolor;
    private Material[] material1;
    private float buttonDownTimer;
    private float delay = 1f;
    public GameObject go;
    public Stack<GameObject> LastSelected = new Stack<GameObject>();
    public Stack<Material> StandardCol = new Stack<Material>();
    public Vector3 endPosition;
    
    
    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        LaserPointer();
    }

    private void LaserPointer() {
        
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        int raycastLength = 5000;
        endPosition = transform.position + (transform.forward * raycastLength);
        
        if(OVRInput.GetDown(OVRInput.Button.Four)) {
            int countShips = LastSelected.Count;
            for (int i = 0; i <= countShips; i++) {
                GameObject gotmp = LastSelected.Pop();
                material1[1] = StandardCol.Pop();
                gotmp.GetComponent<Collider>().enabled = true;
                gotmp.GetComponent<Selected>().MySelection();
                gotmp.GetComponent<Renderer>().material.SetFloat("_Outline", 0);
                gotmp.transform.GetComponent<Renderer>().materials = material1;
            }
        }
        
        
        
        if(OVRInput.GetDown(OVRInput.Button.Two)) {

            GameObject gotmp = LastSelected.Pop();
            material1[1] = StandardCol.Pop();
            gotmp.GetComponent<Collider>().enabled = true;
            gotmp.GetComponent<Selected>().MySelection();
            gotmp.GetComponent<Renderer>().material.SetFloat("_Outline", 0);
            gotmp.transform.GetComponent<Renderer>().materials = material1;
        }

        if (Physics.Raycast(ray, out hit, raycastLength) && hit.transform.CompareTag(SELECTABLE)) {
            lastHitTransform = hit.transform;
            hit.transform.GetComponent<Renderer>().material.SetFloat("_Outline", 0.2f);
            go = hit.transform.gameObject;
            endPosition = hit.point;



            if (OVRInput.GetDown(OVRInput.Button.One)) {
                
                go.GetComponent<Selected>().MySelection();
                go.GetComponent<Collider>().enabled = false;
                material1 = hit.transform.GetComponent<MeshRenderer>().materials;
                Material mat1 = material1[1];
                StandardCol.Push(mat1);
                material1[1] = invisiblemat;
                hit.transform.GetComponent<MeshRenderer>().materials = material1;
                //standardcolor = hit.transform.GetComponent<Renderer>().materials[1].color;
                //hit.transform.GetComponent<Renderer>().materials[1].color = Color.clear;
                LastSelected.Push(go);
            }
            
            if (OVRInput.Get(OVRInput.Button.One)) {
                    buttonDownTimer += Time.deltaTime;
                    if (buttonDownTimer > delay) {
                        go.GetComponent<Selected>().MySelection();
                        go.GetComponent<Collider>().enabled = false;
                        material1 = hit.transform.GetComponent<MeshRenderer>().materials;
                        Material mat1 = material1[1];
                        StandardCol.Push(mat1);
                        material1[1] = invisiblemat;
                        hit.transform.GetComponent<MeshRenderer>().materials = material1;
                        LastSelected.Push(go); 
                    }
            }
        }
        
        else if (go != null && go.GetComponent<Selected>().isSelected == false) {
            lastHitTransform.GetComponent<Renderer>().material.SetFloat("_Outline", 0);
            
        }

        
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    private void BubbleSelect() {
        targetLength = targetLength + OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y * rayScalingSpeed;
        if (targetLength <= 0) {
            targetLength = 0;
        }
    }
}