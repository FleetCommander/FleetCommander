using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[ExecuteInEditMode]
public class UfoEditing : MonoBehaviour {
    // Start is called before the first frame update

    [SerializeField] private UfoColors ufoColors;
    private Color color;
    
    // Update is called once per frame
    void Update() {
        switch (ufoColors) {
            case UfoColors.RED:
                GetComponentInChildren<Renderer>().sharedMaterials[1].color = Color.red;
                break;
            case UfoColors.YELLOW:
                GetComponentInChildren<Renderer>().sharedMaterials[1].color = Color.yellow;
                break;
            case UfoColors.BLUE:
                GetComponentInChildren<Renderer>().sharedMaterials[1].color = Color.blue;
                break;
        }
    }
}
